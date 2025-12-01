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
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(employee.Password);
             employee.Password = hashedPassword;

            var result = await _unitOfWork.employeeService.Register(employee);
            if (result > 0)
                return Ok("Employee registered successfully");

            return BadRequest("Failed to register employee");

        }

       


       


        
    }
}
