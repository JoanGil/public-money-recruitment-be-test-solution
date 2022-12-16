namespace VacationRental.Core.Models.Application
{
    public class RentalModel
    {
        public int Id { get; set; }
        public List<UnitModel>? Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
