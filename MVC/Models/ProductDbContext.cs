﻿using Microsoft.EntityFrameworkCore;

namespace MVC.Models
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Определите конфигурацию для моделей, если необходимо
            // Например, modelBuilder.Entity<Product>().ToTable("ClientCatalogImages");
        }
    }
}
