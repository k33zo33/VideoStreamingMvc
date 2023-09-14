using AutoMapper;
using Humanizer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MoviesRWA.BL.Repositories;
using MoviesRWA.WEB.Services;
using MoviesRWA.WEB.ViewModels;
using NuGet.Protocol.Plugins;
using System.Diagnostics;
using System.Security.Claims;

namespace MoviesRWA.WEB.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public UserController(ILogger<UserController> logger, IUserRepo userRepo, IMapper mapper, IEmailService emailService)
        {
            _logger = logger;
            _userRepo = userRepo;
            _mapper = mapper;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            var blUsers = _userRepo.GetAll();
            var vmUsers = _mapper.Map<IEnumerable<VMUser>>(blUsers);

            return View(vmUsers);
        }
        public IActionResult Register() 
        { 
            var countries = _userRepo.GetAllCountries();
            var viweModelRegister = new VMRegister { Countries = countries };

            return View(viweModelRegister);
        }


        [HttpPost]
        public IActionResult Register(VMRegister register)
        {
            var countries = _userRepo.GetAllCountries();
            register.Countries = countries;

            if (!ModelState.IsValid) { return View(register); }
            if (_userRepo.CheckUsernameIfExists(register.Username))
            {
                ModelState.AddModelError("Username", "That username already exists");
                return View(register);

            }
            if (_userRepo.CheckEmailIfExists(register.Email))
            {
                ModelState.AddModelError("Username", "That username already exists");
            }

            _userRepo.CreateUser(
                register.Username,
                register.FirstName,
                register.LastName,
                register.Email,
                register.Password,
                register.Phone,
                register.CountryOfResidenceId
                );





            return RedirectToAction("UserRegistered");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(VMLogin login)
        {


            if (!ModelState.IsValid) return View(login);

            var user = _userRepo.GetConfirmedUser(login.Username, login.Password);
            if (user != null)
            {
                ModelState.AddModelError("Login","You have entered invalid username or password");
            }


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims);

            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = login.StayLoggedIn
            };

            //HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity),authenticationProperties).Wait();


            if (login.UrlRedirect != null)
            {
                return Redirect(login.UrlRedirect);
            }
            else
            {
                return RedirectToAction("Index"); 
            }
        }

        public IActionResult EmailValidation(VMEmailValidation validation)
        {
            if (!ModelState.IsValid)
                return View(validation);

            _userRepo.ConfirmEmail(validation.Email, validation.SecurityToken);


            return RedirectToAction("Login");
        }

    }
}