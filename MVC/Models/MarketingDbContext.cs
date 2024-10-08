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
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>()
                .HasOne(c => c.Section)
                .WithMany(s => s.Contacts)
                .HasForeignKey(c => c.SectionId);
        }
    }
}

