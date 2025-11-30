using Loan___Emi_Repayment.DATAACCESS.IRepository;
using Loan___Emi_Repayment.MODELS;
using Microsoft.AspNetCore.Mvc;
using Loan___Emi_Repayment.UTILITY;
using Microsoft.AspNetCore.Authorization;

namespace Loan___Emi_Repayment.Controllers
{
    [ApiController]
    [Authorize]

    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;


        public EmployeeController(IUnitOfWork unitOfWork, IConfiguration config )
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

        [HttpPost]
        [Route("Employee-Registration")]
        public async Task<IActionResult> AddDetails(Employee employee)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            // TODO: Hash password using BCrypt
            employee.Password = employee.Password;

            var result = await _unitOfWork.employeeService.Register(employee);
            if (result > 0)
                return Ok("Employee registered successfully");

            return BadRequest("Failed to register employee");

        }

       


        [HttpPost]
        [Route("Employee-Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            var result = await _unitOfWork.employeeService.Login(login.EmailId, login.Password);

            if (result == null) return Unauthorized("Invalid Email or Password");

            var token = JwtTokenHelper.GenerateToken(result.EmailId, _config);

            return Ok(new
            {
                Message = "Login Successful",
                Token = token,
                Role = result.Role

            });
        }


        
    }
}
