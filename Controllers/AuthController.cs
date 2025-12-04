using Loan___Emi_Repayment.DATAACCESS.IRepository;
using Loan___Emi_Repayment.MODELS;
using Loan___Emi_Repayment.UTILITY;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loan___Emi_Repayment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public AuthController(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

       
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

              
                var result = await _unitOfWork.authService.Login(login.EmailId, login.Password);


                if(result == null)
                {
                    throw new Exception("Invalid User Details");
                }
                bool isCorrect = BCrypt.Net.BCrypt.Verify(login.Password, result.Password);
                
                if (isCorrect == false)
                {
                    return NotFound(new { Message = "Invalid Email or Password" });
                }

                // Generate JWT Token
                var token = JwtTokenHelper.GenerateToken(result.EmailId, result.Role, _config);

                return Ok(new
                {
                    Message = "Login Successful",
                    Token = token,
                    UserDetail = result
                });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}
