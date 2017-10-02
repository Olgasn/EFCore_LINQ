using Microsoft.EntityFrameworkCore;

namespace EFCore_LINQ.Models
{
    public class FuelContext: DbContext
    {
        public FuelContext(DbContextOptions<FuelContext> options): base(options)
        {
        }
        public DbSet<Fuel> Fuels { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Tank> Tanks { get; set; }
    }
}
