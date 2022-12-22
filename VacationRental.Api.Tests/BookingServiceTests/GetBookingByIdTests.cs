using FluentAssertions;

using System.Linq;
using System.Net;
using System.Threading.Tasks;

using VacationRental.Api.Tests.BaseTests;
using VacationRental.Api.Tests.Helpers;
using VacationRental.Core.Models.Application;
using VacationRental.Core.Models.Response;
using VacationRental.Core.Services;

using Xunit;

namespace VacationRental.Api.Tests.BookingServiceTests
{
    public class GetBookingByIdTests : BaseBookingServiceTest
    {
        private readonly BookingService bookingService;

        public GetBookingByIdTests()
        {
            bookingService = GetService();
        }

        [Fact]
        public async Task GetBookingById_NoBookingOnTheDatabase_ReturnsNotFoundResponseModel()
        {
            // Arrange
            var expectedResponse = new ResponseModel<BookingModel>(HttpStatusCode.NotFound, "Booking not found");

            // Act
            var actualResponse = await bookingService.GetBookingById(FakerExtensions.GenerateRandomNumber(1));

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }

        [Fact]
        public async Task GetBookingById_NoBookingIdOnTheDatabase_ReturnsNotFoundResponseModel()
        {
            // Arrange
            _ = await GetAndAddFakeBookings(10);
            var expectedResponse = new ResponseModel<BookingModel>(HttpStatusCode.NotFound, "Booking not found");

            // Act
            var actualResponse = await bookingService.GetBookingById(FakerExtensions.GenerateRandomNumber(3));

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }

        [Fact]
        public async Task GetBookingById_BookingWithId_ReturnsResponseModelWithBooking()
        {
            // Arrange
            var insertedRental = (await GetAndAddFakeRentals(1)).First();

            // Act
            var actualResponse = await bookingService.GetBookingById(insertedRental.Bookings.First().Id);

            // Assert
            actualResponse.Validation.IsValid.Should().BeTrue();
            actualResponse.Data.Should().NotBeNull();
        }
    }
}
