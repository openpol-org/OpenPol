using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenPol.WebAPI.Models.Request;
using OpenPol.WebAPI.Models.Response;
using OpenPol.WebAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenPol.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    

    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService) 
        { 
            _userService = userService; 
        }

        [HttpPost("login")]
        public IActionResult Authenticate([FromBody] AuthRequest model)
        {
            Response response = new Response();

            var userResponse = _userService.Auth(model);

            if (userResponse == null)
            {
                response.Success = 0;
                response.Mensaje = "Wrong user or password.";
                return BadRequest();
            }


            response.Success = 1;
            response.Data = userResponse;
           
            return Ok(response);
        }

       
    }
}
