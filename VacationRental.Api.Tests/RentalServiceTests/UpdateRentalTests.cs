using FluentAssertions;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using VacationRental.Api.Tests.BaseTests;
using VacationRental.Api.Tests.Helpers;
using VacationRental.Core.Models.Api;
using VacationRental.Core.Models.Application;
using VacationRental.Core.Models.Response;
using VacationRental.Core.Services;
using VacationRental.Infrastructure.Data;

using Xunit;

namespace VacationRental.Api.Tests.RentalServiceTests
{
    public class UpdateRentalTests : BaseRentalServiceTest
    {
        private readonly RentalService rentalService;

        public UpdateRentalTests()
        {
            rentalService = GetService();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public async Task UpdateRental_RentalIdIsZeroOrLess_ReturnsBadRequestResponseModel(int rentalId)
        {
            // Arrange
            var expectedResponse = new ResponseModel(HttpStatusCode.BadRequest, "RentalId cannot be zero");

            // Act
            var actualResponse = await rentalService.UpdateRental(rentalId, null);

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }

        [Fact]
        public async Task UpdateRental_ModelIsNull_ReturnsBadRequestResponseModel()
        {
            // Arrange
            var expectedResponse = new ResponseModel(HttpStatusCode.BadRequest, "Model cannot be null");

            // Act
            var actualResponse = await rentalService.UpdateRental(FakerExtensions.GenerateRandomNumber(1), null);

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public async Task UpdateRental_InvalidUnitsValue_ReturnsBadRequestResponseModel(int units)
        {
            // Arrange
            var model = new RentalUpdateModel
            {
                Units = units,
                PreparationTimeInDays = FakerExtensions.GenerateRandomNumber(1)
            };
            var expectedResponse = new ResponseModel(HttpStatusCode.BadRequest, "Units must be greater than 0");

            // Act
            var actualResponse = await rentalService.UpdateRental(FakerExtensions.GenerateRandomNumber(1), model);

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public async Task UpdateRental_InvalidPrepartionTimeValue_ReturnsBadRequestResponseModel(int preparationTimeInDays)
        {
            // Arrange
            var model = new RentalUpdateModel
            {
                Units = FakerExtensions.GenerateRandomNumber(1),
                PreparationTimeInDays = preparationTimeInDays
            };
            var expectedResponse = new ResponseModel(HttpStatusCode.BadRequest, "PreparationTimeInDays must be greater than 0");

            // Act
            var actualResponse = await rentalService.UpdateRental(FakerExtensions.GenerateRandomNumber(1), model);

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }

        [Fact]
        public async Task UpdateRental_RentalDoesNotExist_ReturnsNotFoundResponseModel()
        {
            // Arrange
            var model = new RentalUpdateModel
            {
                Units = FakerExtensions.GenerateRandomNumber(1),
                PreparationTimeInDays = FakerExtensions.GenerateRandomNumber(1)
            };
            var expectedResponse = new ResponseModel(HttpStatusCode.NotFound, "Rental not found");

            // Act
            var actualResponse = await rentalService.UpdateRental(FakerExtensions.GenerateRandomNumber(1), model);

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }

        [Fact]
        public async Task UpdateRental_MoreBookingsThanUnits_ReturnsBadRequestResponseModel()
        {
            // Arrange
            var rental = (await GetAndAddFakeRentals(1)).First();
            _ = await GetAndAddFakeBookings(rental.Id, FakerExtensions.GenerateRandomNumber(2));

            var model = new RentalUpdateModel
            {
                Units = 1,
                PreparationTimeInDays = FakerExtensions.GenerateRandomNumber(1)
            };
            var expectedResponse = new ResponseModel(HttpStatusCode.BadRequest, "Cannot reduce the number of units below the number of bookings");

            // Act
            var actualResponse = await rentalService.UpdateRental(rental.Id, model);

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }

        [Fact]
        public async Task UpdateRental_BookingWithValidValues_ReturnsRentalIdInResponseModel()
        {
            // Arrange
            var rental = (await GetAndAddFakeRentals(1)).First();

            rental.Bookings = null;
            await context.SaveChangesAsync();

            var model = new RentalUpdateModel
            {
                Units = 1,
                PreparationTimeInDays = FakerExtensions.GenerateRandomNumber(1)
            };
            var expectedResponse = new ResponseModel(rental.Id);

            // Act
            var actualResponse = await rentalService.UpdateRental(rental.Id, model);

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }
    }
}
