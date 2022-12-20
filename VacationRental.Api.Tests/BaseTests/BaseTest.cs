using AutoFixture;



using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using VacationRental.Api.Tests.Helpers;
using VacationRental.Infrastructure;
using VacationRental.Infrastructure.Data;

namespace DBoxx.Tests.Base
{
    public class BaseTest : IDisposable
    {
        private bool disposed = false;
        public readonly VacationRentalContext context;
        private int bookingId = 1;
        private int rentalId = 1;
        private int unitId = 1;

        public BaseTest()
        {
            var options = new DbContextOptionsBuilder<VacationRentalContext>()
                   .UseInMemoryDatabase(Guid.NewGuid().ToString())
                   .Options;

            context = new VacationRentalContext(options);
        }

        protected List<Booking> GetFakeBookings(int count, int? createdRentalId = null)
        {
            var bookings = new Fixture()
                                   .Build<Booking>()
                                   .Do(o =>
                                   {
                                       o.Id = bookingId++;
                                       o.RentalId = createdRentalId ?? 0;
                                       o.Start = DateTime.Now;
                                       o.Nights = FakerExtensions.GenerateRandomNumber(1);
                                       o.Unit = GetFakeUnits(1).FirstOrDefault();
                                   })
                                   .OmitAutoProperties()
                                   .CreateMany(count).ToList();

            return bookings;
        }

        protected async Task<List<Booking>> GetAndAddFakeBookings(int count, int? createdRentalId = null)
        {
            var bookings = GetFakeBookings(count, createdRentalId);
            context.Booking.AddRange(bookings);
            await context.SaveChangesAsync();
            return bookings;
        }

        protected List<Rental> GetFakeRentals(int count)
        {
            var rentals = new Fixture()
                                   .Build<Rental>()
                                   .Do(o =>
                                   {
                                       o.Id = rentalId++;
                                       o.PreparationTimeInDays = FakerExtensions.GenerateRandomNumber(1);
                                   })
                                   .OmitAutoProperties()
                                   .CreateMany(count).ToList();

            return rentals;
        }

        protected async Task<List<Rental>> GetAndAddFakeRentals(int count)
        {
            var rentals = GetFakeRentals(count);
            context.Rental.AddRange(rentals);
            await context.SaveChangesAsync();
            return rentals;
        }

        protected List<Unit> GetFakeUnits(int count)
        {
            var units = new Fixture()
                                   .Build<Unit>()
                                   .Do(o =>
                                   {
                                       o.Id = unitId++;
                                   })
                                   .OmitAutoProperties()
                                   .CreateMany(count).ToList();

            return units;
        }

        protected async Task<List<Unit>> GetAndAddFakeUnits(int count)
        {
            var units = GetFakeUnits(count);
            context.Unit.AddRange(units);
            await context.SaveChangesAsync();
            return units;
        }

        public void Dispose()
        {
            if (disposed)
                return;

            context?.Database.EnsureDeleted();
            context?.Dispose();

            disposed = true;

            GC.SuppressFinalize(this);
        }
    }
}
