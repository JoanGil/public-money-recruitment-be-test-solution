using VacationRental.Core.Interfaces;
using VacationRental.Core.Models.Api;
using VacationRental.Core.Models.Application;
using VacationRental.Infrastructure;
using VacationRental.Infrastructure.Data;

namespace VacationRental.Core.Services
{
    public class RentalService : IRentalService
    {
        private readonly VacationRentalContext context;

        public RentalService(VacationRentalContext context)
        {
            this.context = context;
        }

        public async Task<RentalModel> GetRentalById(int rentalId)
        {
            var rental = await context.Rental.FindAsync(rentalId);
            if (rental == null)
                return null;

            return new RentalModel
            {
                Id = rental.Id,
                Units = rental.Units.Select(u => new UnitModel { Id = u.Id, RentalId = rentalId }).ToList(),
                PreparationTimeInDays = rental.PreparationTimeInDays
            };
        }

        public async Task<RentalModel> AddRental(RentalBindingModel model)
        {
            var rental = new Rental
            {
                Id = model.Id,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            };

            await context.Rental.AddAsync(rental);
            await context.SaveChangesAsync();

            return new RentalModel
            {
                Id = rental.Id,
                Units = rental.Units,
                PreparationTimeInDays = rental.PreparationTimeInDays
            };
        }
    }
}
