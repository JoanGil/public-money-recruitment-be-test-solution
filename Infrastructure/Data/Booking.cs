using System.ComponentModel.DataAnnotations.Schema;

namespace VacationRental.Infrastructure.Data
{
    public class Booking
    {
        public int Id { get; set; }

        [ForeignKey("RentalId")]
        public int RentalId { get; set; }
        public Rental? Rental { get; set; }

        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
