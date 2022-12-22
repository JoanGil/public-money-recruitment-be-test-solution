using System.Linq.Expressions;

using VacationRental.Core.Models.Api;
using VacationRental.Core.Models.Application;
using VacationRental.Core.Models.Response;
using VacationRental.Infrastructure.Data;

namespace VacationRental.Core.Interfaces
{
    public interface IBookingService
    {
        Task<ResponseModel<BookingModel>> GetBookingById(int bookingId);
        Task<ResponseModel<List<BookingModel>>> GetAllBookings(int? rentalId = null, Expression<Func<Booking, bool>> expression = null);
        Task<ResponseModel> CreateBooking(BookingCreateModel model);
    }
}
