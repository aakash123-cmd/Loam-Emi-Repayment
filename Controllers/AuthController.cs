using Loan___Emi_Repayment.UTILITY;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Loan___Emi_Repayment.UTILITY;


namespace StudentAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(LoginRequest request)
        {
            // TODO: Validate from DB
            if (request.Email == "sagar@gmail.com" && request.Password == "123456")
            {
                var token = JwtTokenHelper.GenerateToken(request.Email, _config);
                return Ok(new { token });
            }

            return Unauthorized(new { message = "Invalid credentials" });
        }


        [HttpGet("secure-data")]
        public IActionResult SecureData()
        {
            return Ok("You accessed secured API!");
        }
    }

}
