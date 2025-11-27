using Loan___Emi_Repayment.DATAACCESS.IRepository;
using Loan___Emi_Repayment.DATAACCESS.Repository;
using Loan___Emi_Repayment.MODELS;
using Microsoft.AspNetCore.Mvc;

namespace Loan___Emi_Repayment.Controllers
{
    public class LoanController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public LoanController(IUnitOfWork unitOfWork)
        {
        _unitOfWork = unitOfWork;
        }




        [HttpPost]
        [Route("AddLoan")]
        public async Task<IActionResult> Create(Loan loan)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var result = await _unitOfWork.loanService.AddDetails(loan);

            return Ok(result);


        }

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyPayment([FromBody] PaymentHistory ph)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            if (ph.AmountPaid <= 0)return BadRequest("Amount must be greater than zero");

            
            var result = await _unitOfWork.loanService.ApplyLoanPayment(ph.CustomerLoanId, ph.AmountPaid, ph.fkPaidBy);

            if (result == null) return NotFound("Loan not found");

            return Ok(result);
        }

    }
}
