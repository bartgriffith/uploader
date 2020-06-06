using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Uploader.Models;
using Uploader.Services;

namespace Uploader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }


        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            return Ok(_customerService.GetAllCustomerNames());
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] Customer customer)
        {
            var response = await _customerService.CreateCustomer(customer);

            return Ok($"Errors Occurred: {response.ErrorsOccurred}");
        }
    }
}
