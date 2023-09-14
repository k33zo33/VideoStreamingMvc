using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesRWA.Administration.ViewModels;
using MoviesRWA.BL.BLModels;
using MoviesRWA.BL.Repositories;

namespace MoviesRWA.Administration.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepo _userRepo;
        private readonly ICountryRepo _countryRepo;
        private readonly IMapper _mapper;

        public UserController(IUserRepo userRepo, ICountryRepo countryRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _countryRepo = countryRepo;
            _mapper = mapper;
        }

        public IActionResult Index(string userName, string firstName, string lastName, string coutryName)
        {
            IEnumerable<BLUser> blUsers;

            blUsers = _userRepo.GetFilteredData(userName, firstName, lastName, coutryName);

            var vmUsers = _mapper.Map<IEnumerable<VMUser>>(blUsers);

            return View(vmUsers);
        }

        public IActionResult Registration() 
        {
            var countries = _countryRepo.GetAll();
            ViewBag.Countries = countries;
            

            return View();
        }

        [HttpPost]
        public IActionResult Register(VMRegister register)
        {
            var user = _userRepo.CreateUser(
                register.Username,
                register.FirstName,
                register.LastName,
                register.Email,
                register.Password,
                register.Phone,
                register.CountryOfResidenceId);

            return RedirectToAction("Index");
        }
    }
}
