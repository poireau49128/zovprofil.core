using Microsoft.EntityFrameworkCore;

namespace MVC.Models
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public string GetConnectionString()
		{
			return Database.GetDbConnection().ConnectionString;
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
