using Microsoft.EntityFrameworkCore;

namespace MVC.Models
{
    public class MarketingDbContext : DbContext
    {
        public MarketingDbContext(DbContextOptions<MarketingDbContext> options) : base(options)
        {
        }
        public DbSet<New> News { get; set; }
        public DbSet<Dealer> Dealers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Определите конфигурацию для моделей, если необходимо
            // Например, modelBuilder.Entity<Product>().ToTable("ClientCatalogImages");
        }
    }
}

