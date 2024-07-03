using Microsoft.EntityFrameworkCore;
using ProductManagementApp.DAL.Models;
using System.Text.Json;

namespace ProductManagement.DAL
{
    public class ProductManagementAppContext : DbContext
    {
        public ProductManagementAppContext(DbContextOptions<ProductManagementAppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
            .Property(p => p.Images)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<byte[]>>(v, (JsonSerializerOptions)null)
            );
        }
    }
}
