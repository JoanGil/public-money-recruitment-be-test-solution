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
    public class BaseCalendarServiceTest : BaseTest
    {
        protected Mock<IRentalService> rentalServiceMock;
        protected Mock<IBookingService> bookingServiceMock;

        public BaseCalendarServiceTest()
        {
            rentalServiceMock = new Mock<IRentalService>();
            bookingServiceMock = new Mock<IBookingService>();
        }

        protected CalendarService GetService()
        {
            return new CalendarService(context, rentalServiceMock.Object, bookingServiceMock.Object);
        }
    }
}
