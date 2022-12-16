using VacationRental.Core.Models.Api;
using VacationRental.Core.Models.Application;

namespace VacationRental.Core.Interfaces
{
    public interface IRentalService
    {
        Task<RentalModel> GetRentalById(int rentalId);
        Task<RentalModel> CreateRental(RentalBindingModel rental);
    }
}
