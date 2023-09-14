using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesRWA.BL.Repositories;
using MoviesRWA.WEB.ViewModels;

namespace MoviesRWA.WEB.Controllers
{
    public class VideoController : Controller
    {

        private readonly IVideoRepo _videoRepo;
        private readonly IGenreRepo _genreRepo;
        private readonly IImageRepo _imageRepo;
        private readonly IMapper _mapper;

        public VideoController(IVideoRepo videoRepo, IGenreRepo genreRepo, IImageRepo imageRepo, IMapper mapper)
        {
            _videoRepo = videoRepo;
            _genreRepo = genreRepo;
            _imageRepo = imageRepo;
            _mapper = mapper;
        }


        //[Authorize] --- dodaj kasnije kad rijesis autorizac do jaja
        public IActionResult Index(int page, int size, string videoName)
        {
            if (size ==0)  size = 10; 

            var blVideos = _videoRepo.GetPagedVideoName(page, size, videoName);
            var vmVideos = _mapper.Map<IEnumerable<VMVideo>>(blVideos);

            ViewData["page"] = page; 
            ViewData["size"] = size;

            ViewData["pages"] = _videoRepo.GetTotalCount() / size;


            return View(vmVideos);
        }
    }
}
