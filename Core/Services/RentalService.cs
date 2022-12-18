using Microsoft.EntityFrameworkCore;

using System.Net;

using VacationRental.Core.Interfaces;
using VacationRental.Core.Models.Api;
using VacationRental.Core.Models.Application;
using VacationRental.Core.Models.Response;
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

        public async Task<ResponseModel<RentalModel>> GetRentalById(int rentalId)
        {
            var rental = await context.Rental.Include(r => r.Units).Where(r => r.Id == rentalId).FirstOrDefaultAsync();
            if (rental == null)
                return new ResponseModel<RentalModel>(HttpStatusCode.NotFound, "Rental not found");

            var response = new ResponseModel<RentalModel>
            {
                Data = new RentalModel
                {
                    Id = rental.Id,
                    Units = rental.Units.Select(u => new UnitModel { Id = u.Id }).ToList(),
                    PreparationTimeInDays = rental.PreparationTimeInDays
                }
            };

            return response;
        }

        public async Task<ResponseModel> CreateRental(RentalRequestModel model)
        {
            if (model == null)
                return new ResponseModel(HttpStatusCode.BadRequest, "Model cannot be null");

            var rental = new Rental
            {
                PreparationTimeInDays = model.PreparationTimeInDays
            };

            await context.Rental.AddAsync(rental);

            var units = Enumerable.Range(1, model.Units).Select(u => new Unit { RentalId = rental.Id }).ToList();
            await context.Unit.AddRangeAsync(units);

            await context.SaveChangesAsync();

            return new ResponseModel { Data = rental.Id };
        }
    }
}
