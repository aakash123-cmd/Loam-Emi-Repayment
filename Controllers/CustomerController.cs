using Loan___Emi_Repayment.DATAACCESS.IRepository;
using Loan___Emi_Repayment.MODELS;
using Microsoft.AspNetCore.Mvc;

namespace Loan___Emi_Repayment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerController(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("AddCustomer")]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
           var result = await _unitOfWork.customerService.AddDetails(customer);
            return Ok(result);
        }

       




        [HttpPost]
        [Route("AddCustomersLoan")]
        public async Task<IActionResult> Create(Customer_Loan_ReqParams cl_req)
        {
            if(!ModelState.IsValid) return ValidationProblem(ModelState);   
            var result = await _unitOfWork.customerService.AddDetails(cl_req);
            return Created("New Loan Taken",result);
        }



        [HttpGet]
        [Route("GetLoanDetailsByCustomerId")]
public async Task<IActionResult> GetDetails(int id)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var result = await _unitOfWork.customerService.GetLoanDetailsByCustomerId(id);
            if (result == null) return NotFound();
            return Ok(result);
        }




        //[HttpGet]
        //[Route("GetCustomerById")]
        //public async Task<IActionResult> GetById(int Id)
        //{
        //    if (!ModelState.IsValid) return ValidationProblem(ModelState);
        //    var result = await _unitOfWork.customerService.GetById(Id);
        //    if(result == null) return NotFound();
        //    return Ok(result);
        //}
    }
}
