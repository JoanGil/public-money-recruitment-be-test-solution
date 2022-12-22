using FluentAssertions;

using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using VacationRental.Api.Tests.BaseTests;
using VacationRental.Api.Tests.Helpers;
using VacationRental.Core.Models.Application;
using VacationRental.Core.Models.Response;
using VacationRental.Core.Services;
using VacationRental.Infrastructure.Data;

using Xunit;

namespace VacationRental.Api.Tests.RentalServiceTests
{
    public class GetRentalByIdTests : BaseRentalServiceTest
    {
        private readonly RentalService rentalService;

        public GetRentalByIdTests()
        {
            rentalService = GetService();
        }

        [Fact]
        public async Task GetRentalById_NoRentalOnTheDatabase_ReturnsNotFoundResponseModel()
        {
            // Arrange
            var expectedResponse = new ResponseModel<RentalModel>(HttpStatusCode.NotFound, "Rental not found");

            // Act
            var actualResponse = await rentalService.GetRentalById(FakerExtensions.GenerateRandomNumber(1));

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }

        [Fact]
        public async Task GetRentalById_RentalOnTheDatabase_ReturnsRentalWithResponseModel()
        {
            // Arrange
            var rental = (await GetAndAddFakeRentals(1)).First();
            var expectedResponse = new ResponseModel<RentalModel>
            {
                Data = new RentalModel
                {
                    Id = rental.Id,
                    Units = rental.Units.Select(u => new UnitModel { Id = u.Id }).ToList(),
                    PreparationTimeInDays = rental.PreparationTimeInDays
                }
            };

            // Act
            var actualResponse = await rentalService.GetRentalById(rental.Id);

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }
    }
}
