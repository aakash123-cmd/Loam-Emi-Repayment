using Loan___Emi_Repayment.DATAACCESS.IRepository;
using Loan___Emi_Repayment.MODELS;
using Microsoft.AspNetCore.Mvc;

namespace Loan___Emi_Repayment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnquiryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public EnquiryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpPost]
        [Route("Add-Customer-Came-For-Enquiry")]
        public async Task<IActionResult> Create( Enquiry enquiry)
        {
           if(!ModelState.IsValid) return ValidationProblem(ModelState);
            var result = await _unitOfWork.enquiryService.AddDetails(enquiry);
            return Ok(result);

        }


        [HttpGet]
        [Route("Display-All-Customers-Enquiry")]
        public async Task<List<Enquiry>> DisplayDetails() => await _unitOfWork.enquiryService.GetDetails();



        [HttpGet]
        [Route("Display-Customer-Enquiry-ById")]
        public async Task<IActionResult> DisplayDetailsById(int id)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _unitOfWork.enquiryService.GetDetails(id);
            return Ok(result);
        }



        [HttpPatch]
        [Route("Update-Enquiry-Details-ById")]
        public async Task<IActionResult> Update(Enquiry enquiry)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var result = await _unitOfWork.enquiryService.UpdateDetails(enquiry);
            return Ok(result);  

        }

       

    }
}
