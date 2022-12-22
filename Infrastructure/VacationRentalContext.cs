using Microsoft.EntityFrameworkCore;

using VacationRental.Infrastructure.Data;

namespace VacationRental.Infrastructure
{
    public class VacationRentalContext : DbContext
    {
        public VacationRentalContext(DbContextOptions<VacationRentalContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("VacationRentalDb");
        }

        public DbSet<Rental> Rental { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Unit> Unit { get; set; }
    }
}
