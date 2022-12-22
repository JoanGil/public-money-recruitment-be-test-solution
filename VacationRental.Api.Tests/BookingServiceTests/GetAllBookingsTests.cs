using FluentAssertions;

using System.Collections.Generic;
using System.Threading.Tasks;

using VacationRental.Api.Tests.BaseTests;
using VacationRental.Core.Models.Application;
using VacationRental.Core.Services;

using Xunit;

namespace VacationRental.Api.Tests.BookingServiceTests
{
    public class GetAllBookingsTests : BaseBookingServiceTest
    {
        private readonly BookingService bookingService;

        public GetAllBookingsTests()
        {
            bookingService = GetService();
        }

        [Fact]
        public async Task GetAllBookings_NoFilter_ReturnsAllBookingsResponseModel()
        {
            // Arrange
            var bookingCount = 10;
            _ = await GetAndAddFakeBookings(bookingCount);

            // Act
            var actualResponse = await bookingService.GetAllBookings();

            // Assert
            actualResponse.Data.Should().NotBeNull();
            actualResponse.Data.Count.Should().Be(bookingCount);
            actualResponse.Data.GetType().Should().Be(typeof(List<BookingModel>));
        }

        [Fact]
        public async Task GetAllBookings_WithRentalIdFilter_ReturnsFilteredByRentalIdBookingsResponseModel()
        {
            var bookingGroupCount = 2;

            _ = await GetAndAddFakeBookings(bookingGroupCount, 1);
            _ = await GetAndAddFakeBookings(bookingGroupCount, 2);
            _ = await GetAndAddFakeBookings(bookingGroupCount, 3);

            // Act
            var actualResponse = await bookingService.GetAllBookings(2);

            // Assert
            actualResponse.Data.Should().NotBeNull();
            actualResponse.Data.Count.Should().Be(bookingGroupCount);
            actualResponse.Data.GetType().Should().Be(typeof(List<BookingModel>));
        }
    }
}
