using AutoMapper;
using Integration.BLModels;
using Integration.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesRWA.Integration.BLModels;

namespace MoviesRWA.Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly RwaMoviesContext _dbContext;
        private readonly IMapper _mapper;

        public GenreController(RwaMoviesContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public ActionResult<BLGenreResp> Get(int id)
        {
            try
            {
                var dbGenre = _dbContext.Genres.FirstOrDefault(x => x.Id == id);
                if (dbGenre == null)
                    return NotFound($"Could not find genre (id): {id}");
                var blGenre = _mapper.Map<BLGenreResp>(dbGenre);

                return Ok(blGenre);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Can't fetch the requested data");
            }
        }


        [HttpGet("[action]")]
        public ActionResult<IEnumerable<BLGenreResp>> GetAll()
        {
            try
            {
                var dbGenres = _dbContext.Genres;
                var blGenres = _mapper.Map<IEnumerable<BLGenreResp>>(dbGenres);
                return Ok(blGenres);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Can't fetch the requested data");
            }
        }

        [HttpPost()]
        public ActionResult<BLGenreResp> Post(BLGenre genre)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var dbGenre = _mapper.Map<Genre>(genre);
                var entity = _dbContext.Genres.Add(dbGenre);
                _dbContext.SaveChanges();


                var dbResponseGenre = entity.Entity;

                var blGenre = _mapper.Map<BLGenreResp>(dbResponseGenre);

                return Ok(blGenre);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<BLGenreResp> Put(int id, BLGenre genre)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var dbGenre = _dbContext.Genres.FirstOrDefault(x => x.Id == id);
                if (dbGenre == null)
                    return NotFound($"Could not find genre with id {id}");

                dbGenre.Name = genre.Name;
                dbGenre.Description = genre.Description;

                _dbContext.SaveChanges();
                var blGenre = _mapper.Map<BLGenreResp>(dbGenre);

                return Ok(blGenre);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Can't fetch the requested data");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<BLGenreResp> Delete(int id)
        {
            try
            {
                var dbGenre = _dbContext.Genres.FirstOrDefault(x => x.Id == id);
                if (dbGenre == null)
                    return NotFound($"Could not find genre with id {id}");

                _dbContext.Genres.Remove(dbGenre);

                _dbContext.SaveChanges();
                var blGenre = _mapper.Map<BLGenreResp>(dbGenre);

                return Ok(blGenre);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Can't fetch the requested data");
            }
        }

    }
}
