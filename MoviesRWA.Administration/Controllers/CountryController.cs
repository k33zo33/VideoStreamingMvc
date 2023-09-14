using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesRWA.Administration.ViewModels;
using MoviesRWA.BL.Repositories;
using System.Security.Policy;

namespace MoviesRWA.Administration.Controllers
{
    public class CountryController : Controller
    {

        private readonly ICountryRepo _countryRepo;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepo countryRepo, IMapper mapper)
        {
            _countryRepo = countryRepo;
            _mapper = mapper;
        }

        public IActionResult Index(int page, int size)
        {
            if (size ==0)
            {
                size = 10;
            }

            var blCountry = _countryRepo.GetPaged(page, size);
            var vmCountry = _mapper.Map<IEnumerable<VMCountry>>(blCountry);


            ViewData["page"] = page;
            ViewData["size"]=size;
            ViewData["pages"] = _countryRepo.CountryCount() / size;

            return View(vmCountry);


           
        }
    }
}
