using Microsoft.AspNetCore.Mvc;

using System.Net;
using System.Threading.Tasks;

using VacationRental.Api.Models.Response;
using VacationRental.Core.Interfaces;
using VacationRental.Core.Models.Api;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService bookingService;

        public BookingsController(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public async Task<IActionResult> GetBookingById(int bookingId)
        {
            var response = await bookingService.GetBookingById(bookingId);
            if (response.HttpStatusCode != HttpStatusCode.OK)
                return StatusCode((int)response.HttpStatusCode, response.Validation.Message);

            return Ok(response.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var response = await bookingService.GetAllBookings();
            if (response.HttpStatusCode != HttpStatusCode.OK)
                return StatusCode((int)response.HttpStatusCode, response.Validation.Message);

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(BookingCreateModel model)
        {
            var response = await bookingService.CreateBooking(model);
            if (response.HttpStatusCode != HttpStatusCode.OK)
                return StatusCode((int)response.HttpStatusCode, response.Validation.Message);

            return Ok(new ResourceIdViewModel { Id = (int)response.Data });
        }
    }
}
