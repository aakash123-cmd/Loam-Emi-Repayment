using Loan___Emi_Repayment.DATAACCESS.IRepository;
using Loan___Emi_Repayment.MODELS;
using Microsoft.AspNetCore.Mvc;
using Loan___Emi_Repayment.UTILITY;

namespace Loan___Emi_Repayment.Controllers
{
    public class EmployeeController : Controller
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
            return Ok(new
            {
                Message = "Employee Registered Successfully",
                EmployeeId = result.EmployeeId

            });

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
