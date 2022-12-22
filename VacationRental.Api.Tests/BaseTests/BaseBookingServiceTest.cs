using DBoxx.Tests.Base;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VacationRental.Core.Interfaces;
using VacationRental.Core.Services;

namespace VacationRental.Api.Tests.BaseTests
{
    public class BaseBookingServiceTest : BaseTest
    {
        protected Mock<IRentalService> rentalServiceMock;

        public BaseBookingServiceTest()
        {
            rentalServiceMock = new Mock<IRentalService>();
        }

        protected BookingService GetService()
        {
            return new BookingService(context, rentalServiceMock.Object);
        }
    }
}
