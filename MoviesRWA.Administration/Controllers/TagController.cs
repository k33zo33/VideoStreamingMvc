using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesRWA.Administration.ViewModels;
using MoviesRWA.BL.BLModels;
using MoviesRWA.BL.Repositories;

namespace MoviesRWA.Administration.Controllers
{
    public class TagController : Controller
    {

        private readonly ITagRepo _tagRepo;
        private readonly IMapper _mapper;

        public TagController(ITagRepo tagRepo, IMapper mapper)
        {
            _tagRepo = tagRepo;
            _mapper = mapper;
        }


        public IActionResult Index(int page, int size)
        {

            if (size == 0)
            {
                size = 10;
            }

            var blTags = _tagRepo.GetPagedTags(page, size);
            var vmTag = _mapper.Map<IEnumerable<VMTag>>(blTags);


            ViewData["page"] = page;
            ViewData["size"] = size;
            ViewData["pages"] = _tagRepo.GetTotalCount() / size;


            return View(vmTag);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(VMTag vmTag)
        {
            if(!ModelState.IsValid)
            {
                return View(vmTag);
            }

            var tag = _mapper.Map<BLTag>(vmTag);
            _tagRepo.CreateTag(tag);

            return RedirectToAction("Index");
            

        }

        public ActionResult Edit(int id)
        {
            var blTag = _tagRepo.GetById(id);
            var vmTag = _mapper.Map<VMTag>(blTag);



            return View(vmTag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, VMTag tag)
        {
            try
            {
                var blTag = _mapper.Map<BLTag>(tag);
                _tagRepo.UpdateTag(blTag.Id, blTag);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Delete(int id)
        {
            var blTag = _tagRepo.GetById(id);
            var vmTag = _mapper.Map<VMTag>(blTag);

            return View(vmTag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _tagRepo.DeleteTag(id);

                return RedirectToAction(nameof(Index));
            }
            catch 
            {

                return View();
            }
        }




    }
}
