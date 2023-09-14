using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesRWA.Administration.ViewModels;
using MoviesRWA.BL.BLModels;
using MoviesRWA.BL.Repositories;

namespace MoviesRWA.Administration.Controllers
{
    public class VIdeoController : Controller
    {

        private readonly IVideoRepo _videoRepo;
        private readonly IGenreRepo _genreRepo;
        private readonly IImageRepo _imageRepo;
        private readonly IMapper _mapper;

        public VIdeoController(IVideoRepo videoRepo, IGenreRepo genreRepo, IImageRepo imageRepo, IMapper mapper)
        {
            _videoRepo = videoRepo;
            _genreRepo = genreRepo;
            _imageRepo = imageRepo;
            _mapper = mapper;
        }

        public IActionResult Index(int page, int size, int? selectedVidID, int? selectedGenreId)
        {
            if (size == 0)
            {
                size = 10;
            }

            var blVideos = _videoRepo.GetPagedVideo(page, size, selectedVidID, selectedGenreId);
            var vmVideos = _mapper.Map<IEnumerable<VMVideo>>(blVideos);

            ViewData["page"] = page;
            ViewData["size"] = size;
            ViewData["pages"] = _videoRepo.GetTotalCount() / size;


            return View(vmVideos);
        }

        public ActionResult Create()
        {
            var dbVideo = _videoRepo.GetAllGenres();


            var vmVideo = _mapper.Map<VMVideo>(dbVideo);


            return View(vmVideo);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMVideo video)
        {
            try
            {
                video.ImageId = _imageRepo.CreateImage(video.ImageUpload);

                var dbVideo = _mapper.Map<BLVideo>(video);
                _videoRepo.CreateVideo(dbVideo);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Details(int id)
        {
            var dbVideo = _videoRepo.GetVideoById(id);
            var vmVideo = _mapper.Map<VMVideo>(dbVideo);

            return View(vmVideo);
        }


        public ActionResult Edit(int id)
        {
            var dbVideo = _videoRepo.GetVideoById(id);
            var vmVideo = _mapper.Map<VMVideo>(dbVideo);
            var genres = _genreRepo.GetAll();
            ViewBag.Genres = genres;

            return View(vmVideo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, VMVideo video)
        {
            try
            {
                var dbVideo = _mapper.Map<BLVideo>(video);
                _videoRepo.UpdateVideo(dbVideo.Id, dbVideo);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Delete(int id)
        {
            var video = _videoRepo.GetVideoById(id);
            var blService = _mapper.Map<VMVideo>(video);

            return View(blService);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _videoRepo.DeleteVideo(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



    }
}
