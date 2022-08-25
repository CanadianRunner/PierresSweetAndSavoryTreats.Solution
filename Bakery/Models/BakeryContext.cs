using Microsoft.EntityFrameworkCore;

namespace Bakery.Models
{
  public class BakeryContext : IdentityDbContext<UserId>
  {
    public DbSet<Flavour> Flavours { get; set; }
    public DbSet<Treat> Treats { get; set; }
    public DbSet<FlavourTreat> FlavourTreat { get; set; }

    public BakeryContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseLazyLoadingProxies();
    }
  }
}