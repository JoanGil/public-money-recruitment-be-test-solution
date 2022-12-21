using System.Net;

using VacationRental.Core.Interfaces;
using VacationRental.Core.Models.Application;
using VacationRental.Core.Models.Response;
using VacationRental.Infrastructure;

namespace VacationRental.Core.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly VacationRentalContext context;
        private readonly IRentalService rentalService;
        private readonly IBookingService bookingService;

        public CalendarService(VacationRentalContext context, IRentalService rentalService, IBookingService bookingService)
        {
            this.context = context;
            this.rentalService = rentalService;
            this.bookingService = bookingService;
        }

        public async Task<ResponseModel<CalendarModel>> GenerateBookingCalendar(int rentalId, DateTime start, int nights)
        {
            if (nights <= 0)
                return new ResponseModel<CalendarModel>(HttpStatusCode.BadRequest, "Nights value must be positive");

            var rentalResponse = await rentalService.GetRentalById(rentalId);
            if (rentalResponse == null || rentalResponse.HttpStatusCode == HttpStatusCode.NotFound)
                return new ResponseModel<CalendarModel>(HttpStatusCode.NotFound, "Rental not found");

            var result = new CalendarModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateModel>()
            };

            var calendarDates = Enumerable.Range(0, nights)
                .Select(day => new CalendarDateModel
                {
                    Date = start.Date.AddDays(day),
                    Bookings = new List<CalendarBookingModel>(),
                });

            foreach (var calendarDate in calendarDates)
            {
                var bookingsResponse = await bookingService.GetAllBookings(rentalId, booking => booking.Start <= calendarDate.Date && booking.Start.AddDays(booking.Nights) > calendarDate.Date);
                if (bookingsResponse.Data.Any())
                {
                    calendarDate.Bookings = bookingsResponse.Data.Select(booking => new CalendarBookingModel
                    {
                        Id = booking.Id,
                        Unit = booking.UnitId
                    }).ToList();

                    calendarDate.PreparationTimes = bookingsResponse.Data.Select(booking => new CalendarPreparationTimeModel
                    {
                        Unit = rentalResponse.Data.PreparationTimeInDays
                    }).ToList();
                }

                result.Dates.Add(calendarDate);
            }

            return new ResponseModel<CalendarModel>(result);
        }
    }
}
