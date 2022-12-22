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
    public class CreateRentalTests : BaseRentalServiceTest
    {
        private readonly RentalService rentalService;

        public CreateRentalTests()
        {
            rentalService = GetService();
        }

        [Fact]
        public async Task CreateRental_ModelIsNull_ReturnsBadRequestResponseModel()
        {
            // Arrange
            var expectedResponse = new ResponseModel<RentalModel>(HttpStatusCode.BadRequest, "Model cannot be null");

            // Act
            var actualResponse = await rentalService.CreateRental(null);

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }

        [Fact]
        public async Task CreateRental_ModelIsValid_ReturnsValidResponseModel()
        {
            // Arrange
            var expectedResponse = new ResponseModel<RentalModel>(HttpStatusCode.BadRequest, "Model cannot be null");

            // Act
            var actualResponse = await rentalService.CreateRental(null);

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualResponse);
        }

    }
}
