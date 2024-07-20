using BlazorDemoApp.Shared.Classes.BaseClass;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlazorDemoApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : MyLoggerbaseController
    {
        private ServiceOf_Users _curSer;

        public AuthController(ILogger<AuthController> logger, ServiceOf_Users ServiceOf_Users) : base(logger)
        {
            _curSer = ServiceOf_Users;
        }

        [HttpPost("register")]
        public IActionResult Register(Base0_MyAppUser_Register m)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.SelectMany(kvp => kvp.Value.Errors.Select(e => new ApiError { Field = kvp.Key, Message = e.ErrorMessage })).ToList();
                return BadRequest(new ApiResponse<object>  { Success = false, Message = "Validation errors occurred.", Data = null,  Errors = errors });
            }

            var r = _curSer.Register(m);
            return (r.Success)? Ok(r) : StatusCode(500,r);
        }

        [HttpPost("login")]
        public IActionResult login(Base0_MyAppUser_Login m)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.SelectMany(kvp => kvp.Value.Errors.Select(e => new ApiError { Field = kvp.Key, Message = e.ErrorMessage })).ToList();
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Validation errors occurred.", Data = null, Errors = errors });
            }

            var r = _curSer.login(m);
            return (r.Success) ? Ok(r) : StatusCode(500, r);
        }
    }
}
