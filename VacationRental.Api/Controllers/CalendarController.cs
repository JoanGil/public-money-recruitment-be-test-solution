using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;

using VacationRental.Api.Models;
using VacationRental.Api.Models.Response;
using VacationRental.Infrastructure.Data;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IDictionary<int, Rental> _rentals;
        private readonly IDictionary<int, Booking> _bookings;

        public CalendarController(
            IDictionary<int, Rental> rentals,
            IDictionary<int, Booking> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        [HttpGet]
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                throw new ApplicationException("Nights must be positive");

            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            var rental = _rentals[rentalId];

            var result = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            for (var day = 0; day < nights; day++)
            {
                var calendarDate = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(day),
                    Bookings = new List<CalendarBookingViewModel>(),
                };

                var bookings = _bookings.Values
                    .Where(booking => booking.RentalId == rentalId
                        && booking.Start <= calendarDate.Date && booking.Start.AddDays(booking.Nights) > calendarDate.Date);

                if (bookings.Any())
                {
                    calendarDate.Bookings = bookings.Select(booking => new CalendarBookingViewModel
                    {
                        Id = booking.Id,
                        Unit = rental.Units
                    }).ToList();

                    calendarDate.PreparationTimes = bookings.Select(booking => new CalendarPreparationTimeModel { Unit = rental.PreparationTimeInDays }).ToList();
                }

                result.Dates.Add(calendarDate);
            }

            return result;
        }
    }
}
