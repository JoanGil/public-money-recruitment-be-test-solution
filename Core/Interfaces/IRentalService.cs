using VacationRental.Core.Models.Api;
using VacationRental.Core.Models.Application;
using VacationRental.Core.Models.Response;

namespace VacationRental.Core.Interfaces
{
    public interface IRentalService
    {
        Task<ResponseModel<RentalModel>> GetRentalById(int rentalId);
        Task<ResponseModel> CreateRental(RentalCreateModel rental);
        Task<ResponseModel> UpdateRental(int rentalId, RentalUpdateModel rental);
    }
}
