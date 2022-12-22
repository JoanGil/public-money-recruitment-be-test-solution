using FluentAssertions;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using VacationRental.Api.Tests.BaseTests;
using VacationRental.Api.Tests.Helpers;
using VacationRental.Core.Interfaces;
using VacationRental.Core.Models.Application;
using VacationRental.Core.Models.Response;
using VacationRental.Core.Services;
using VacationRental.Infrastructure.Data;

using Xunit;

namespace VacationRental.Api.Tests.CalendarServiceTests
{
    public class GenerateBookingCalendarTests : BaseCalendarServiceTest
    {
        private readonly CalendarService calendarService;

        public GenerateBookingCalendarTests()
        {
            calendarService = GetService();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public async Task GenerateBookingCalendar_RentalIdLessOrEqualThanZero_ReturnsBadRequestResponseModel(int rentalId)
        {
            // Arrange
            var expectedResponse = new ResponseModel<CalendarModel>(HttpStatusCode.BadRequest, "RentalId cannot be less than or equal to 0");

            // Act
            var actualResponse = await calendarService.GenerateBookingCalendar(rentalId, DateTime.Today.AddDays(1), 1);

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public async Task GenerateBookingCalendar_NightsValueNotPositive_ReturnsBadRequestResponseModel(int nigths)
        {
            // Arrange
            var expectedResponse = new ResponseModel<CalendarModel>(HttpStatusCode.BadRequest, "Nights value must be positive");

            // Act
            var actualResponse = await calendarService.GenerateBookingCalendar(FakerExtensions.GenerateRandomNumber(1), DateTime.Today.AddDays(1), nigths);

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }

        [Fact]
        public async Task GenerateBookingCalendar_StartDateIsLessThanToday_ReturnsBadRequestResponseModel()
        {
            // Arrange
            var start = DateTime.Today.AddDays(-1);
            var expectedResponse = new ResponseModel<CalendarModel>(HttpStatusCode.BadRequest, "Start date must be greater than today");

            // Act
            var actualResponse = await calendarService.GenerateBookingCalendar(FakerExtensions.GenerateRandomNumber(1), start, FakerExtensions.GenerateRandomNumber(1));

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }

        [Fact]
        public async Task GenerateBookingCalendar_RentalIdNotFound_ReturnsNotFoundResponseModel()
        {
            // Arrange
            var start = DateTime.Today.AddDays(1);
            rentalServiceMock.Setup(x => x.GetRentalById(It.IsAny<int>())).ReturnsAsync(new ResponseModel<RentalModel>(HttpStatusCode.NotFound, "Rental not found"));

            var expectedResponse = new ResponseModel<CalendarModel>(HttpStatusCode.NotFound, "Rental not found");

            // Act
            var actualResponse = await calendarService.GenerateBookingCalendar(FakerExtensions.GenerateRandomNumber(1), start, FakerExtensions.GenerateRandomNumber(1));

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }

        [Fact]
        public async Task GenerateBookingCalendar_RentalNotFound_ReturnsNullResponseAndNotFoundResponseModel()
        {
            // Arrange
            var start = DateTime.Today.AddDays(1);
            rentalServiceMock.Setup(x => x.GetRentalById(It.IsAny<int>())).ReturnsAsync(null as ResponseModel<RentalModel>);

            var expectedResponse = new ResponseModel<CalendarModel>(HttpStatusCode.NotFound, "Rental not found");

            // Act
            var actualResponse = await calendarService.GenerateBookingCalendar(FakerExtensions.GenerateRandomNumber(1), start, FakerExtensions.GenerateRandomNumber(1));

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }

        [Fact]
        public async Task GenerateBookingCalendar_ValidInputOneBooking_ReturnsCalendarModel()
        {
            // Arrange
            var rentalId = 1;
            var start = DateTime.Today.AddDays(1);
            var nights = 3;

            var getByIdResponseModel = new ResponseModel<RentalModel> { Data = new RentalModel { PreparationTimeInDays = 1 } };
            rentalServiceMock.Setup(x => x.GetRentalById(rentalId)).ReturnsAsync(getByIdResponseModel);

            bookingServiceMock.Setup(x => x.GetAllBookings(rentalId, It.IsAny<Expression<Func<Booking, bool>>>()))
                .ReturnsAsync(new ResponseModel<List<BookingModel>>
                {
                    Data = new List<BookingModel>
                    {
                        new BookingModel
                        {
                            Id = 1,
                            UnitId = 2,
                            Start = start.AddDays(1),
                            Nights = 1
                        }
                    }
                });

            // Act
            var actualResult = await calendarService.GenerateBookingCalendar(rentalId, start, nights);

            // Assert
            actualResult.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            actualResult.Data.Should().NotBeNull();
            actualResult.Data.RentalId.Should().Be(rentalId);
            actualResult.Data.Dates.Count.Should().Be(nights);
            actualResult.Data.Dates[1].Bookings.Count.Should().Be(1);
            actualResult.Data.Dates[1].PreparationTimes.Count.Should().Be(1);
            actualResult.Data.Dates[1].PreparationTimes[0].Unit.Should().Be(1);
        }
    }
}
