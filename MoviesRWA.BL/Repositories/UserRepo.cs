using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoviesRWA.BL.BLModels;
using MoviesRWA.BL.DALModels;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MoviesRWA.BL.Repositories
{

    //we need this for DI!!
    public interface IUserRepo
    {
        public IEnumerable<BLUser> GetAll();
        BLUser CreateUser(string username, string firstName, string lastName, string email, string password, string phoneNumber, int countryId);
        void ConfirmEmail(string email, string securityToken);
        BLUser GetConfirmedUser(string username, string password);
        void ChangePassword(string username, string newPassword);
        BLUser GetUserById(int id);
        IEnumerable<BLUser> GetFilteredUserDataName(string term);
        IEnumerable<BLUser> GetFilteredUserFirstName(string term);
        IEnumerable<BLUser> GetFilteredUserLastName(string term);
        IEnumerable<BLCountry> GetFilteredUserCountry(string term);
        IEnumerable<BLUser> GetFilteredData(string username, string firstName, string lastName, string country);
        void SoftDeleteUser(int id);
        bool CheckUsernameIfExists(string username);
        bool CheckEmailIfExists(string email);
        void UpdateUser(int id, BLUser user);
        IEnumerable<SelectListItem> GetAllCountries();

    }
    public class UserRepo : IUserRepo
    {
        private readonly RwaMoviesContext _dbContext;
        private readonly IMapper _mapper;
        public UserRepo(RwaMoviesContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void ChangePassword(string username, string newPassword)
        {
            var selectedUser = _dbContext.Users.FirstOrDefault(x =>
                x.Username == username &&
                !x.DeletedAt.HasValue);

            (var salt, var b64Salt) = GenerateSalt();

            var b64Hash = CreateHash(newPassword, salt);

            selectedUser.PwdHash = b64Hash;
            selectedUser.PwdSalt = b64Salt;

            _dbContext.SaveChanges();
        }

        public bool CheckEmailIfExists(string email) => _dbContext.Users.Any(x => x.Email == email && x.DeletedAt == null);

        public bool CheckUsernameIfExists(string username) => _dbContext.Users.Any(x => x.Username == username && x.DeletedAt == null);

        public void ConfirmEmail(string email, string securityToken)
        {
            var userToConfirm = _dbContext.Users.FirstOrDefault(x =>
               x.Email == email &&
               x.SecurityToken == securityToken &&
               x.DeletedAt == null);

            userToConfirm.IsConfirmed = true;

            _dbContext.SaveChanges();
        }

        public BLUser CreateUser(string username, string firstName, string lastName, string email, string password, string phoneNumber, int countryId)
        {
            (var salt, var b64Salt) = GenerateSalt();

            var b64Hash = CreateHash(password, salt);
            var b64SecurityToken = GenerateSecurityToken();

            var dbUser = new User()
            {
                CreatedAt = DateTime.UtcNow,
                Username = username,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PwdHash = b64Hash,
                PwdSalt = b64Salt,
                SecurityToken = b64SecurityToken,
                Phone = phoneNumber,
                CountryOfResidenceId = countryId
            };
            _dbContext.Users.Add(dbUser);

            _dbContext.SaveChanges();

            var blUser = _mapper.Map<BLUser>(dbUser);
            return blUser;
        }

        private static string GenerateSecurityToken()
        {
            byte[] securityToken = RandomNumberGenerator.GetBytes(256 / 8);
            string b64SecToken = Convert.ToBase64String(securityToken);

            return b64SecToken;
        }

        private static string CreateHash(string password, byte[] salt)
        {
            byte[] hash =
               KeyDerivation.Pbkdf2(
                   password: password,
                   salt: salt,
                   prf: KeyDerivationPrf.HMACSHA256,
                   iterationCount: 100000,
                   numBytesRequested: 256 / 8);
            string b64Hash = Convert.ToBase64String(hash);

            return b64Hash;
        }

        private static (byte[], string) GenerateSalt()
        {
            var salt = RandomNumberGenerator.GetBytes(128 / 8);
            var b64Salt = Convert.ToBase64String(salt);

            return (salt, b64Salt);
        }

        public IEnumerable<BLUser> GetAll()
        {
            // First retrieve collection from database
            var dbUsers = _dbContext.Users;
            //Then map it to BL model collection
            var blUsers = _mapper.Map<IEnumerable<BLUser>>(dbUsers);

            return blUsers;

            //LOOK INTO THIS LATER!<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<--
            //return _dbContext.Users
            //  .Include(u => u.CountryOfResidence)
            //  .ToList()
            //  .Select(u => _mapper.Map<BLUser>(u));
        }

        public IEnumerable<SelectListItem> GetAllCountries()
        {
            var country = _dbContext.Countries.ToList();
            var selectListItems = country.Select(g => new SelectListItem
            {
                Value = g.Id.ToString(), // Convert the Id to a string
                Text = g.Name
            });


            return selectListItems;
        }

        public BLUser GetConfirmedUser(string username, string password)
        {
            var dbUser = _dbContext.Users.FirstOrDefault(x =>x.Username == username && x.IsConfirmed == true);

            var salt = Convert.FromBase64String(dbUser.PwdSalt);
            var b64Hash = CreateHash(password, salt);

            if (dbUser.PwdHash != b64Hash) { return null; }
               

            var blUser = _mapper.Map<BLUser>(dbUser);

            return blUser;
        }

        public IEnumerable<BLUser> GetFilteredData(string username, string firstName, string lastName, string country)
        {
            var dbUsers = _dbContext.Users.Include(u => u.CountryOfResidence)
                .Select(u=>new
                { 
                    User = u,
                    CountryName = u.CountryOfResidence.Name,
                }).AsEnumerable();
            dbUsers = dbUsers.Where(v => v.User.DeletedAt == null);
            dbUsers = dbUsers.Where(v=> v.User.IsConfirmed !=false);
            if (!string.IsNullOrEmpty(username))
            {
                dbUsers = dbUsers.Where(v => v.User.Username.Contains(username));
            }
            if (!string.IsNullOrEmpty(firstName))
            {
                dbUsers = dbUsers.Where(v => v.User.FirstName.Contains(firstName));
            }
            if (!string.IsNullOrEmpty(country))
            {
                dbUsers = dbUsers.Where(v => v.User.CountryOfResidence.Name.Contains(country));
            }

            var blUser = _mapper.Map<IEnumerable<BLUser>>(dbUsers.Select(v=>v.User));

            return blUser;
        }

        public IEnumerable<BLCountry> GetFilteredUserCountry(string term)
        {
            var dbCountires = _dbContext.Countries.Where(u => u.Name.Contains(term)).AsEnumerable();

            var blCountires = _mapper.Map<IEnumerable<BLCountry>>(dbCountires);

            return blCountires;
        }

        public IEnumerable<BLUser> GetFilteredUserDataName(string term)
        {
            var dbUsers = _dbContext.Users.Where(u => u.Username.Contains(term)).AsEnumerable();

            dbUsers = dbUsers.Where(v => v.DeletedAt == null);
            dbUsers = dbUsers.Where(v => v.IsConfirmed != false);

            var blUsers = _mapper.Map<IEnumerable<BLUser>>(dbUsers);

            return blUsers;
        }

        public IEnumerable<BLUser> GetFilteredUserFirstName(string term)
        {
            var dbUsers = _dbContext.Users.Where(u => u.FirstName.Contains(term)).AsEnumerable();

            dbUsers = dbUsers.Where(v => v.DeletedAt == null);
            dbUsers = dbUsers.Where(v => v.IsConfirmed != false);

            var blUsers = _mapper.Map<IEnumerable<BLUser>>(dbUsers);

            return blUsers;
        }

        public IEnumerable<BLUser> GetFilteredUserLastName(string term)
        {
            var dbUsers = _dbContext.Users.Where(u => u.LastName.Contains(term)).AsEnumerable();
            dbUsers = dbUsers.Where(v => v.DeletedAt == null);
            dbUsers = dbUsers.Where(v => v.IsConfirmed != false);
            var blUsers = _mapper.Map<IEnumerable<BLUser>>(dbUsers);

            return blUsers;
        }

        public BLUser GetUserById(int id)
        {
            try
            {

                var dbUser = _dbContext.Users
               .Include(u => u.CountryOfResidence)
               .FirstOrDefault(u => u.Id == id);
                if (dbUser == null)
                {
                    throw new ArgumentException("User not found.");
                }

                return _mapper.Map<BLUser>(dbUser);

            }
            catch (Exception e)
            {

                throw new Exception("An error occurred while retrieving the user.", e);
            }
        }

        public void SoftDeleteUser(int id)
        {
            var dbUser = _dbContext.Users.FirstOrDefault(x => x.Id == id);
            dbUser.DeletedAt = DateTime.UtcNow;

            _dbContext.SaveChanges();
        }

        public void UpdateUser(int id, BLUser user)
        {
            try
            {
                var dbUser = _dbContext.Users.FirstOrDefault(v => v.Id == id);
                if (dbUser == null)
                {

                    throw new ArgumentException("User not found.");
                }
                dbUser.FirstName = user.FirstName;
                dbUser.LastName = user.LastName;
                dbUser.Phone = user.Phone;
                dbUser.CountryOfResidenceId = user.CountryOfResidenceId;




                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {

                throw new Exception("An error occurred while updating the user.", e);
            }
        }
    }
}
