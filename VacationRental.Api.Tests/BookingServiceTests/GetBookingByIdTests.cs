using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
        public async Task GetBookingById_BookingWithNoId_ReturnsNotFoundResponseModel()
        {
            // Arrange
            var expectedResponse = new ResponseModel<BookingModel>(HttpStatusCode.NotFound, "Booking not found");

            // Act
            var actualResponse = await bookingService.GetBookingById(FakerExtensions.GenerateRandomNumber(1));

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }
    }
}
