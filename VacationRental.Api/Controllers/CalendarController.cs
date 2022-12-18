using Microsoft.AspNetCore.Mvc;

using System;
using System.Net;
using System.Threading.Tasks;

using VacationRental.Core.Interfaces;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService calendarService;

        public CalendarController(ICalendarService calendarService)
        {
            this.calendarService = calendarService;
        }

        [HttpGet]
        public async Task<IActionResult> GenerateBookingCalendar(int rentalId, DateTime start, int nights)
        {
            var calendarResponse = await calendarService.GenerateBookingCalendar(rentalId, start, nights);

            if (calendarResponse.HttpStatusCode != HttpStatusCode.OK)
                return StatusCode((int)calendarResponse.HttpStatusCode, calendarResponse.Validation.Message);

            return Ok(calendarResponse.Data);
        }
    }
}
