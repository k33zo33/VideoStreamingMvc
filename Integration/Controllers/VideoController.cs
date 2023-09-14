using AutoMapper;
using Integration.BLModels;
using Integration.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MoviesRWA.Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]



    // [Authorize] <---------------- look into adding this to give access only to authorized 


    public class VideoController : ControllerBase
    {
        private readonly RwaMoviesContext _dbContext;
        private readonly IMapper _mapper;

        public VideoController(RwaMoviesContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        public ActionResult<BLVideoResp> Get(int id)
        {
            try
            {
                var dbVideo = _dbContext.Videos.FirstOrDefault(x => x.Id == id);
                if (dbVideo == null)
                    return NotFound($"Could not find genre with id {id}");
                var blVideo = _mapper.Map<BLVideoResp>(dbVideo);
                return Ok(blVideo);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Can't fetch the requested data.");
            }
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<BLVideoResp>> GetAll()
        {
            try
            {
                var dbVideos = _dbContext.Videos;
                var blVideos = _mapper.Map<IEnumerable<BLVideoResp>>(dbVideos);
                return Ok(blVideos);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Can't fetch the requested data.");
            }
        }

        [HttpPost()]
        public ActionResult<BLVideoResp> Post(BLCreateVideo video)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var dbVideo = _mapper.Map<Video>(video);

                var entity = _dbContext.Videos.Add(dbVideo);
                var dbVideoResponse = entity.Entity;
                _dbContext.SaveChanges();

                var videoTags = video.TagIds.Select(tagId => new VideoTag { VideoId = dbVideo.Id, TagId = tagId }).ToList();


                _dbContext.VideoTags.AddRange(videoTags);
                _dbContext.SaveChanges();
                var bLVideo = _mapper.Map<BLVideoResp>(dbVideoResponse);

                return Ok(bLVideo);
            }
            catch (Exception ex)
            {
                var errorMessage = "Can't fetch the requested data: " + ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, errorMessage);
            }
        }


        [HttpPut("{id}")]
        public ActionResult<BLVideoResp> Put(int id, BLVideo video)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var dbVideo = _dbContext.Videos.FirstOrDefault(x => x.Id == id);
                if (dbVideo == null)
                    return NotFound($"Could not find genre with id {id}");

                dbVideo.Name = video.Name;
                dbVideo.Description = video.Description;
                dbVideo.GenreId = video.GenreId;
                dbVideo.ImageId = video.ImageId;
                dbVideo.StreamingUrl = video.StreamingUrl;


                _dbContext.SaveChanges();
                var bLVideo = _mapper.Map<BLVideoResp>(dbVideo);

                return Ok(bLVideo);
            }
            catch (Exception ex)
            {
                var errorMessage = "Can't fetch the requested data:" + ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, errorMessage);
            }
        }
        [HttpDelete("{id}")]
        public ActionResult<BLVideoResp> Delete(int id)
        {
            try
            {
                var dbVideo = _dbContext.Videos.FirstOrDefault(x => x.Id == id);
                if (dbVideo == null)
                    return NotFound($"Could not find genre with id {id}");

                _dbContext.VideoTags.RemoveRange(_dbContext.VideoTags.Where(x => x.VideoId == id));
                _dbContext.Videos.Remove(dbVideo);

                _dbContext.SaveChanges();
                var blVideo = _mapper.Map<BLVideoResp>(dbVideo);

                return Ok(blVideo);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Can't fetch the requested data.");
            }
        }

        [HttpGet("[action]")]
        public IActionResult Search(
            [FromQuery] string searchPart,
            [FromQuery] string orderBy,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                IQueryable<Video> videos = _dbContext.Videos.Include(v => v.Genre).Include(v => v.VideoTags);

                if (!string.IsNullOrEmpty(searchPart))
                {
                    videos = videos.Where(v => v.Name.Contains(searchPart));
                }

                if (!string.IsNullOrEmpty(orderBy))
                {
                    switch (orderBy.ToLower())
                    {
                        case "id":
                            videos = videos.OrderBy(v => v.Id);
                            break;
                        case "id_desc":
                            videos = videos.OrderByDescending(v => v.Id);
                            break;
                        case "name":
                            videos = videos.OrderBy(v => v.Name);
                            break;
                        case "name_desc":
                            videos = videos.OrderByDescending(v => v.Name);
                            break;
                        case "totaltime":
                            videos = videos.OrderBy(v => v.TotalSeconds);
                            break;
                        case "totaltime_desc":
                            videos = videos.OrderByDescending(v => v.TotalSeconds);
                            break;
                        default:
                            videos = videos.OrderBy(v => v.Id);
                            break;
                    }
                }

                var pagedVideos = videos.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                var blVideo = _mapper.Map<IEnumerable<BLVideoResp>>(pagedVideos);
                return Ok(blVideo);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving videos from the database.");
            }
        }



    }
}
