using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProductManagement.DAL;

namespace ProductManagementApp.DAL
{
    public class ProductManagementContextFactory : IDesignTimeDbContextFactory<ProductManagementAppContext>
    {
        public ProductManagementAppContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ProductManagementAppContext>();
            optionsBuilder.UseSqlite(connectionString);

            return new ProductManagementAppContext(optionsBuilder.Options);
        }
    }
}
