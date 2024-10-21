using Microsoft.EntityFrameworkCore;
using motoRental.Models;

namespace motoRental.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Moto> Motos { get; set; }
        public DbSet<DeliveryGuy> DeliveryGuys { get; set; }
        public DbSet<Rent> Rents { get; set; }
    }
}
