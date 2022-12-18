namespace VacationRental.Core.Models.Application
{
    public class CalendarDateModel
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingModel> Bookings { get; set; }
        public List<CalendarPreparationTimeModel> PreparationTimes { get; set; }
    }
}
