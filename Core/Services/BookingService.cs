using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;
using System.Net;

using VacationRental.Core.Interfaces;
using VacationRental.Core.Models.Api;
using VacationRental.Core.Models.Application;
using VacationRental.Core.Models.Response;
using VacationRental.Infrastructure;
using VacationRental.Infrastructure.Data;

namespace VacationRental.Core.Services
{
    public class BookingService : IBookingService
    {
        private readonly VacationRentalContext context;
        private readonly IRentalService rentalService;

        public BookingService(VacationRentalContext context, IRentalService rentalService)
        {
            this.context = context;
            this.rentalService = rentalService;
        }

        public async Task<ResponseModel<BookingModel>> GetBookingById(int bookingId)
        {
            var booking = await context.Booking.Include(b => b.Unit).Where(b => b.Id == bookingId).FirstOrDefaultAsync();
            if (booking == null)
                return new ResponseModel<BookingModel>(HttpStatusCode.NotFound, "Booking not found");

            var response = new ResponseModel<BookingModel>
            {
                Data = new BookingModel
                {
                    Id = booking.Id,
                    RentalId = booking.RentalId,
                    UnitId = booking.UnitId,
                    Nights = booking.Nights,
                    Start = booking.Start
                }
            };

            return response;
        }

        public async Task<ResponseModel> CreateBooking(BookingCreateModel model)
        {
            if (model == null)
                return new ResponseModel(HttpStatusCode.BadRequest, "Model cannot be null");

            if (model.Nights <= 0)
                return new ResponseModel(HttpStatusCode.BadRequest, "Nights cannot be 0 or less");

            var rentalResponse = await rentalService.GetRentalById(model.RentalId);
            if (rentalResponse.HttpStatusCode == HttpStatusCode.NotFound)
                return new ResponseModel(rentalResponse.HttpStatusCode, rentalResponse.Validation.Message);

            var bookings = await GetAllBookings(model.RentalId);
            var preparation = rentalResponse.Data.PreparationTimeInDays;

            var overlappingBookings = bookings.Data
                .Where((b => (b.Start <= model.Start.Date && b.Start.AddDays(b.Nights) > model.Start.Date)
                    || (b.Start < model.Start.AddDays(model.Nights + preparation) && b.Start.AddDays(b.Nights) >= model.Start.AddDays(model.Nights + preparation))
                    || (b.Start > model.Start && b.Start.AddDays(b.Nights) < model.Start.AddDays(model.Nights + preparation))))
                .ToList();

            if (overlappingBookings.Count >= rentalResponse.Data.Units.Count)
                return new ResponseModel(HttpStatusCode.BadRequest, "No units available for this rental");

            var availableUnit = rentalResponse.Data.Units.FirstOrDefault(u => !overlappingBookings.Any(b => b.UnitId == u.Id));

            var bookingToAdd = context.Add(new Booking
            {
                RentalId = model.RentalId,
                UnitId = availableUnit.Id,
                Nights = model.Nights,
                Start = model.Start
            });
            await context.SaveChangesAsync();

            return new ResponseModel(bookingToAdd.Entity.Id);
        }

        public async Task<ResponseModel<List<BookingModel>>> GetAllBookings(int? rentalId = null, Expression<Func<Booking, bool>> expression = null)
        {
            var bookings = context.Booking.AsQueryable();

            if (rentalId != null)
                bookings = bookings.Where(r => r.RentalId == rentalId);

            if (expression != null)
                bookings = bookings.Where(expression);

            var response = new ResponseModel<List<BookingModel>>
            {
                Data = await bookings.Select(r => new BookingModel
                {
                    Id = r.Id,
                    RentalId = r.RentalId,
                    UnitId = r.UnitId,
                    Nights = r.Nights,
                    Start = r.Start
                }).ToListAsync()
            };

            return response;
        }
    }
}
