using System.ComponentModel.DataAnnotations.Schema;

namespace VacationRental.Infrastructure.Data
{
    public class Unit
    {
        public int Id { get; set; }

        [ForeignKey("RentalId")]
        public int RentalId { get; set; }
        public Rental Rental { get; set; }
    }
}
