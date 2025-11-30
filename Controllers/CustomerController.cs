using Loan___Emi_Repayment.DATAACCESS.IRepository;
using Loan___Emi_Repayment.MODELS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loan___Emi_Repayment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerController(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("Add-Customer")]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
           var result = await _unitOfWork.customerService.AddDetails(customer);
            return Ok(result);
        }

       




        [HttpPost]
        [Route("Add-CustomersLoan")]
        public async Task<IActionResult> Create(Customer_Loan_ReqParams cl_req)
        {
            if(!ModelState.IsValid) return ValidationProblem(ModelState);   
            var result = await _unitOfWork.customerService.AddDetails(cl_req);
            return Created("New Loan Taken",result);
        }



        [HttpGet]
        [Route("Get-LoanDetails-By-CustomerId")]
public async Task<IActionResult> GetDetails(int id)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var result = await _unitOfWork.customerService.GetLoanDetailsByCustomerId(id);
            if (result == null) return NotFound();
            return Ok(result);
        }




        [HttpGet]
        [Route("Display-Customer-Details-ById")]
        public async Task<IActionResult> GetById(int Id)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var result = await _unitOfWork.customerService.GetById(Id);
            if (result == null) return NotFound();
            return Ok(result);
        }


        [HttpGet]
        [Route("Display-All-Customer-Details")]
        public async Task<List<Customer>> GetDetails() => await _unitOfWork.customerService.GetDetails();


        [HttpPatch]
        [Route("Update-CustomerDetails")]
        public async Task<IActionResult> Update(Customer customer)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var result = await _unitOfWork.customerService.UpdateDetails(customer);
            return Ok(result);
        }
    }
}
