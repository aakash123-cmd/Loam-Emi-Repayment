using Loan___Emi_Repayment.DATAACCESS.IRepository;
using Loan___Emi_Repayment.DATAACCESS.Repository;
using Loan___Emi_Repayment.MODELS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loan___Emi_Repayment.Controllers
{
    [ApiController]
  
    [Route("[controller]")]

    public class LoanController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public LoanController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }




        [HttpPost]
        [Route("Add-Loan")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Create(Loan loan)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var result = await _unitOfWork.loanService.AddDetails(loan);

            return Ok(result);


        }

        [HttpPost("RePay-Loan")]
        public async Task<IActionResult> RepayPayment([FromBody] PaymentHistory ph)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            if (ph.AmountPaid <= 0) return BadRequest("Amount must be greater than zero");


            var result = await _unitOfWork.loanService.RepayLoanPayment(ph.CustomerLoanId, ph.AmountPaid, ph.fkPaidBy);

            if (result == null) return NotFound("Loan not found");

            return Ok(result);
        }


        [HttpGet]
        [Route("Get-All-LoanDetail")]
        

        public async Task<List<Loan>> GetDetails() => await _unitOfWork.loanService.GetDetails();


        [HttpPatch]
        [Route("Update-LoanDetail")]
        public async Task<IActionResult> UpdateDetails(Loan loan)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var result = await _unitOfWork.loanService.UpdateDetails(loan);
            return Ok(result);
        }



        [HttpGet]
        [Route("Get-All-Loan-Report")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetReport()
        {
            var result = await _unitOfWork.loanService.GetReports();
            return Ok(result);
        }

    }
}
