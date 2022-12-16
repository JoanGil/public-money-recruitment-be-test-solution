using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using VacationRental.Api.ApiModels;
using VacationRental.Api.Models.Response;
using VacationRental.Infrastructure.Data;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDictionary<int, Rental> _rentals;

        public RentalsController(IDictionary<int, Rental> rentals)
        {
            _rentals = rentals;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<IActionResult> GetRentalById(int rentalId)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            return Ok(_rentals[rentalId]);
        }

        [HttpPost]
        public ResourceIdViewModel AddRental(RentalBindingModel model)
        {
            var key = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };

            _rentals.Add(key.Id, new Rental
            {
                Id = key.Id,
                Units = model.NumberOfUnits,
                PreparationTimeInDays = model.PreparationTimeInDays
            });

            return key;
        }
    }
}
