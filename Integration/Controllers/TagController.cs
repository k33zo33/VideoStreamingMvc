using AutoMapper;
using Integration.BLModels;
using Integration.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoviesRWA.Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly RwaMoviesContext _dbContext;
        private readonly IMapper _mapper;

        public TagController(RwaMoviesContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        public ActionResult<BLTagResp> Get(int id)
        {
            try
            {
                var dbTag = _dbContext.Tags.FirstOrDefault(x => x.Id == id);
                if (dbTag == null)
                    return NotFound($"Could not find tag with id {id}");

                var blTag = _mapper.Map<BLTagResp>(dbTag);
                return Ok(blTag);
            }
            catch (Exception ex)
            {
                var errorMessage = "Can't fetch the requested data: " + ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, errorMessage);
            }
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<BLTagResp>> GetAll()
        {
            try
            {

                var dbTag = _dbContext.Tags;
                var blTag = _mapper.Map<IEnumerable<BLTagResp>>(dbTag);
                return Ok(blTag);
            }
            catch (Exception ex)
            {
                var errorMessage = "Can't fetch the requested data: " + ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, errorMessage);
            }
        }

        [HttpPost()]
        public ActionResult<BLTagResp> Post(BLTag tag)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var dbTag = _mapper.Map<Tag>(tag);
                var entity = _dbContext.Tags.Add(dbTag);
                var dbResponseTag = entity.Entity;
                _dbContext.SaveChanges();

                var blTag = _mapper.Map<BLTagResp>(dbResponseTag);


                return Ok(blTag);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Can't fetch the requested data.");
            }
        }

        [HttpPut("{id}")]
        public ActionResult<BLTagResp> Put(int id, BLTag tag)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var dbTag = _dbContext.Tags.FirstOrDefault(x => x.Id == id);
                if (dbTag == null)
                    return NotFound($"Could not find tag with id {id}");

                dbTag.Name = tag.Name;

                _dbContext.SaveChanges();
                var blTag = _mapper.Map<BLTagResp>(dbTag);

                return Ok(blTag);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Can't fetch the requested data.");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<BLTagResp> Delete(int id)
        {
            try
            {

                var dbTag = _dbContext.Tags.FirstOrDefault(x => x.Id == id);
                if (dbTag == null)
                    return NotFound($"Could not find tag with id {id}");
                _dbContext.VideoTags.RemoveRange(_dbContext.VideoTags.Where(x => x.TagId == id));
                _dbContext.Tags.Remove(dbTag);

                _dbContext.SaveChanges();
                var blTag = _mapper.Map<BLTagResp>(dbTag);
                return Ok(blTag);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Can't fetch the requested data.");
            }
        }


    }
}
