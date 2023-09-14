using AutoMapper;
using Integration.BLModels;
using Integration.Models;
using Integration.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesRWA.Integration.BLModels;
using Newtonsoft.Json.Linq;

namespace MoviesRWA.Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }



        [HttpPost("[action]")]
        public ActionResult<User> Register([FromBody] BLUserRegister request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {

                var newUser = _userRepository.Add(request);
                var blUserResponse = _mapper.Map<BLUserRegisterResp>(newUser);
                return Ok(blUserResponse);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult ValidateEmail([FromBody] BLEmailValidationReq request)
        {
            try
            {

                _userRepository.EmailValidation(request);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult<BLToken> JwtTokens([FromBody] BLUserLogin request)
        {
            try
            {
                return Ok(_userRepository.JwtTokens(request));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
