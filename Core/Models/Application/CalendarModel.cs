namespace VacationRental.Core.Models.Application
{
    public class CalendarModel
    {
        public int RentalId { get; set; }
        public List<CalendarDateModel> Dates { get; set; }
    }
}
