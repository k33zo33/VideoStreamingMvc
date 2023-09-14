using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesRWA.Administration.ViewModels;
using MoviesRWA.BL.BLModels;
using MoviesRWA.BL.Repositories;

namespace MoviesRWA.Administration.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreRepo _genreRepo;
        private readonly IMapper _mapper;

        public GenreController(IGenreRepo genreRepo, IMapper mapper)
        {
            _genreRepo = genreRepo;
            _mapper = mapper;
        }

        public IActionResult Index(int page, int size)
        {

            if (size==0)
            {
                size = 10;
            }

            var blGenres = _genreRepo.GetPagedGenres(page, size);
            var vmGenres = _mapper.Map<IEnumerable<VMGenre>>(blGenres);

            ViewData["page"] = page;
            ViewData["size"] = size;
            ViewData["pages"] = _genreRepo.GetTotalCount() / size;

            return View(vmGenres);

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Create(VMGenre vmGenre)
        {

            if (!ModelState.IsValid)
            {
                return View(vmGenre);
            }
            var blGenre = _mapper.Map<BLGenre>(vmGenre);
            _genreRepo.CreateGenre(blGenre);

            return RedirectToAction("Index");

        }

        public IActionResult Edit(int id)
        {

            var blGenre = _genreRepo.GetGenre(id);
            var vmGenre = _mapper.Map<VMGenre>(blGenre);

            return View(vmGenre);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, VMGenre vmGenre)
        {
            try
            {
                var blGenre = _mapper.Map<BLGenre>(vmGenre);
                _genreRepo.UpdateGenre(blGenre.Id, blGenre);
                return RedirectToAction(nameof(Index));

            }
            catch
            {

                return View();
            }


        }

        public IActionResult Delete(int id) 
        {
            var blGenre = _genreRepo.GetGenre(id);
            var vmGenre = _mapper.Map<VMGenre>(blGenre);

            return View(vmGenre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _genreRepo.DeleteGenre(id);
                
                return RedirectToAction(nameof(Index));
            }
            catch 
            {

                return View();
            }
        }

        public IActionResult Details(int id) 
        {
            var blGenre = _genreRepo.GetGenre(id);
            var vmGenre = _mapper.Map<VMGenre>(blGenre);


            return View(vmGenre);
        }


    }
}
