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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // DB validation
            var employee = await _unitOfWork.employeeService.Login(login.EmailId!, login.Password!);

            if (employee == null)
                return Unauthorized(new { Message = "Invalid Email or Password" });

            // Generate JWT Token
            var token = JwtTokenHelper.GenerateToken(employee.EmailId!, _config);

            return Ok(new
            {
                Message = "Login Successful",
                Token = token,
                Role = employee.Role
            });
        }

        // ----------------------------------------------------
        // CHECK TOKEN / SECURE ENDPOINT
        // ----------------------------------------------------
        [HttpGet("check-auth")]
        [Authorize]
        public IActionResult CheckAuthorization()
        {
            return Ok("Token Valid Hai. Employee Authorized Hai!");
        }
    }
}
