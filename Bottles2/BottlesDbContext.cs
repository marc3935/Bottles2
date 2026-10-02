using Microsoft.EntityFrameworkCore;

namespace Bottles
{
    public class BottlesDbContext : DbContext
    {
        public BottlesDbContext(DbContextOptions<BottlesDbContext> options) : base(options)
        {

        }
        public DbSet<Bottle> Bottles { get; set; }
    }
}
