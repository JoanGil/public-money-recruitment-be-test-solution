using VacationRental.Core.Models.Application;
using VacationRental.Core.Models.Response;

namespace VacationRental.Core.Interfaces
{
    public interface ICalendarService
    {
        Task<ResponseModel<CalendarModel>> GenerateBookingCalendar(int rentalId, DateTime start, int nights);
    }
}
