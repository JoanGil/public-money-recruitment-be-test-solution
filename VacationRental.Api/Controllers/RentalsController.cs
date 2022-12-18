using Microsoft.AspNetCore.Mvc;

using System.Net;
using System.Threading.Tasks;

using VacationRental.Api.Models.Response;
using VacationRental.Core.Interfaces;
using VacationRental.Core.Models.Api;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService rentalService;

        public RentalsController(IRentalService rentalService)
        {
            this.rentalService = rentalService;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<IActionResult> GetRentalById(int rentalId)
        {
            var response = await rentalService.GetRentalById(rentalId);
            if (response.HttpStatusCode != HttpStatusCode.OK)
                return StatusCode((int)response.HttpStatusCode, response.Validation.Message);

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRental(RentalRequestModel model)
        {
            var response = await rentalService.CreateRental(model);
            if (response.HttpStatusCode != HttpStatusCode.OK)
                return StatusCode((int)response.HttpStatusCode, response.Validation.Message);

            return Ok(new ResourceIdViewModel { Id = (int)response.Data });
        }
    }
}
