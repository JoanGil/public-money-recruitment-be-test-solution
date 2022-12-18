namespace VacationRental.Infrastructure.Data
{
    public class Rental
    {
        public int Id { get; set; }
        public int PreparationTimeInDays { get; set; }

        public List<Unit>? Units { get; set; }
        public List<Booking>? Bookings { get; set; }
    }
}
