using Integration.BLModels;
using Integration.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using MoviesRWA.Integration.BLModels;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Integration.Repos
{
    public interface IUserRepository
    {
        User Add(BLUserRegister request);
        void EmailValidation(BLEmailValidationReq request);
        BLToken JwtTokens(BLUserLogin request);
    }

    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly RwaMoviesContext _dbContext;

        public UserRepository(IConfiguration configuration, RwaMoviesContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public User Add(BLUserRegister request)
        {
            var userName = request.Username.ToLower().Trim();
            if (_dbContext.Users.Any(x => x.Username.Equals(userName)))
                throw new InvalidOperationException("Username already exists");


            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
            string b64Salt = Convert.ToBase64String(salt);

            byte[] hash =
                KeyDerivation.Pbkdf2(
                    password: request.Password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8);
            string b64Hash = Convert.ToBase64String(hash);


            byte[] securityToken = RandomNumberGenerator.GetBytes(256 / 8);
            string b64SecToken = Convert.ToBase64String(securityToken);




            var newUser = new User
            {
                CreatedAt = DateTime.UtcNow,
                Username = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                IsConfirmed = false,
                SecurityToken = b64SecToken,
                PwdSalt = b64Salt,
                PwdHash = b64Hash,
                CountryOfResidenceId = request.CountryOfResidenceId,

            };
            _dbContext.Users.Add(newUser);
            CreateUserNotification(newUser);
            _dbContext.SaveChanges();
            return newUser;
        }

        private void CreateUserNotification(User user)
        {
            var notif= new Notification()
            {
                CreatedAt = DateTime.UtcNow,
                ReceiverEmail = user.Email,
                Subject = "Please confirm your email adress",
                Body = user.SecurityToken,
                SentAt = null
            };
            _dbContext.Notifications.Add(notif);
            _dbContext.SaveChanges();
        }

        public void EmailValidation(BLEmailValidationReq request)
        {
            var target = _dbContext.Users.FirstOrDefault(x =>
               x.Username == request.Username && x.SecurityToken == request.B64SecToken);

            if (target == null)
                throw new InvalidOperationException("Authentication failed");

            target.IsConfirmed = true;
            _dbContext.SaveChanges();
        }

        public BLToken JwtTokens(BLUserLogin request)
        {
            var isAuthenticated = Authenticate(request.Username, request.Password);

            if (!isAuthenticated)
                throw new InvalidOperationException("Authentication failed");

            // Get secret key bytes
            var jwtKey = _configuration["JWT:Key"];
            var jwtKeyBytes = Encoding.UTF8.GetBytes(jwtKey);



            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, request.Username),
                    new Claim(JwtRegisteredClaimNames.Sub, request.Username),

                }),
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(jwtKeyBytes),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var serializedToken = tokenHandler.WriteToken(token);

            return new BLToken
            {
                Token = serializedToken
            };
        }

        private bool Authenticate(string username, string password)
        {
            var target = _dbContext.Users.Single(x => x.Username == username);

            if (!target.IsConfirmed)
                return false;

            byte[] salt = Convert.FromBase64String(target.PwdSalt);
            byte[] hash = Convert.FromBase64String(target.PwdHash);

            byte[] calcHash =
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8);

            return hash.SequenceEqual(calcHash);
        }
    }
}
