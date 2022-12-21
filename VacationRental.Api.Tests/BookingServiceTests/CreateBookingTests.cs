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

namespace VacationRental.Api.Tests.BookingServiceTests
{
    public class CreateBookingTests : BaseBookingServiceTest
    {
        private readonly BookingService bookingService;

        public CreateBookingTests()
        {
            bookingService = GetService();
        }

        [Fact]
        public async Task CreateBooking_ModelIsNull_ReturnsBadRequestResponseModel()
        {
            // Arrange
            var expectedResponse = new ResponseModel<BookingModel>(HttpStatusCode.BadRequest, "Model cannot be null");

            // Act
            var actualResponse = await bookingService.CreateBooking(null);

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public async Task CreateBooking_NightsIsLessThanOne_ReturnsBadRequestResponseModel(int nights)
        {
            // Arrange
            var model = new BookingCreateModel
            {
                RentalId = 1,
                Nights = nights,
                Start = new System.DateTime(2020, 1, 1)
            };
            var expectedResponse = new ResponseModel<BookingModel>(HttpStatusCode.BadRequest, "Nights cannot be 0 or less");

            // Act
            var actualResponse = await bookingService.CreateBooking(model);

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }

        [Fact]
        public async Task CreateBooking_RentalDoesNotExist_ReturnsNotFoundResponseModel()
        {
            // Arrange
            var model = new BookingCreateModel
            {
                RentalId = FakerExtensions.GenerateRandomNumber(1),
                Nights = 1,
                Start = new System.DateTime(2020, 1, 1)
            };
            var expectedResponse = new ResponseModel<BookingModel>(HttpStatusCode.NotFound, "Rental not found");

            // Act
            var actualResponse = await bookingService.CreateBooking(model);

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }

        [Fact]
        public async Task CreateBooking_OverlappingBookingsFullRental_ReturnsBadRequestResponseModel()
        {
            // Arrange
            var rental = await AddInitialRentalData(addBookings: true);
            var rentalModel = new RentalModel
            {
                Id = rental.Id,
                PreparationTimeInDays = FakerExtensions.GenerateRandomNumber(1),
                Units = rental.Units.Select(u => new UnitModel { Id = u.Id }).ToList()
            };

            var responseRentalModel = new ResponseModel<RentalModel>(rentalModel);
            rentalServiceMock.Setup(r => r.GetRentalById(It.IsAny<int>())).ReturnsAsync(responseRentalModel);

            var model = new BookingCreateModel
            {
                RentalId = rental.Id,
                Nights = FakerExtensions.GenerateRandomNumber(1),
                Start = DateTime.UtcNow.AddDays(1)
            };
            var expectedResponse = new ResponseModel(HttpStatusCode.BadRequest, "No units available for this rental");

            // Act
            var actualResponse = await bookingService.CreateBooking(model);

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }

        [Fact]
        public async Task CreateBooking_NoOverlappingBookings_ReturnsBookingIdResponseModel()
        {
            // Arrange
            var rental = await AddInitialRentalData(addBookings: false);

            var rentalModel = new RentalModel
            {
                Id = rental.Id,
                PreparationTimeInDays = FakerExtensions.GenerateRandomNumber(1),
                Units = rental.Units.Select(u => new UnitModel { Id = u.Id }).ToList()
            };
            var responseRentalModel = new ResponseModel<RentalModel>(rentalModel);
            rentalServiceMock.Setup(r => r.GetRentalById(It.IsAny<int>())).ReturnsAsync(responseRentalModel);

            var model = new BookingCreateModel
            {
                RentalId = rental.Id,
                Nights = FakerExtensions.GenerateRandomNumber(1),
                Start = DateTime.UtcNow.AddDays(1)
            };

            // Act
            var actualResponse = await bookingService.CreateBooking(model);
            var insertedBooking = context.Rental.Where(r => r.Id == rental.Id).Select(r => r.Bookings.First()).First();
            var expectedResponse = new ResponseModel(insertedBooking.Id);

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }

        private async Task<Rental> AddInitialRentalData(bool addBookings = false)
        {
            var rental = new Rental
            {
                PreparationTimeInDays = FakerExtensions.GenerateRandomNumber(1),
                Units = new List<Unit> { new Unit(), new Unit() }
            };

            var rentalEntity = context.Rental.Add(rental);
            await context.SaveChangesAsync();

            if (addBookings)
            {
                var booking1 = new Booking
                {
                    Nights = FakerExtensions.GenerateRandomNumber(1) + 1,
                    RentalId = rentalEntity.Entity.Id,
                    UnitId = rentalEntity.Entity.Units.First().Id,
                    Start = DateTime.UtcNow.AddDays(1)
                };

                var booking2 = new Booking
                {
                    Nights = FakerExtensions.GenerateRandomNumber(1) + 1,
                    RentalId = rentalEntity.Entity.Id,
                    UnitId = rentalEntity.Entity.Units.Skip(1).Take(1).First().Id,
                    Start = DateTime.UtcNow.AddDays(1)
                };

                context.Booking.AddRange(booking1, booking2);
                await context.SaveChangesAsync();
            }

            return rental;
        }
    }
}
