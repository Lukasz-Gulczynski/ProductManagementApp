using Microsoft.Extensions.DependencyInjection;
using ProductManagement.DAL;
using ProductManagementApp.DAL;
using ProductManagementApp.DAL.Interfaces;
using ProductManagementApp.DAL.Models;
using ProductManagementApp.DAL.Repositories;
using ProductManagementApp.Enums;
using ProductManagementApp.Interfaces;
using ProductManagementApp.Mappers;
using ProductManagementApp.Services;
using ProductManagementApp.ViewModels;
using System.Windows;

namespace ProductManagementApp
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            InitializeDatabase();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ProductManagementAppContext>();

            services.AddSingleton<ProductManagementContextFactory>();
            services.AddSingleton<ProductService>();

            services.AddScoped<ISupplierRepository, SupplierRepository>();

            services.AddTransient<IDictionary<SupplierType, IProductMapper>>(provider =>
            {
                var mappers = new Dictionary<SupplierType, IProductMapper>
                {
                    { SupplierType.Supplier1, new Supplier1ProductMapper() },
                    { SupplierType.Supplier2, new Supplier2ProductMapper() },
                    { SupplierType.Supplier3, new Supplier3ProductMapper() }
                };

                return mappers;
            });

            services.AddTransient(provider =>
            {
                var factory = provider.GetRequiredService<ProductManagementContextFactory>();
                return factory.CreateDbContext(null);
            });

            services.AddTransient<MainWindow>();
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<XmlProductLoader>();
        }

        private void InitializeDatabase()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ProductManagementAppContext>();

                if (!dbContext.Suppliers.Any())
                {
                    // Jeśli baza danych jest pusta, dodajemy dostawców
                    dbContext.Suppliers.AddRange(new[]
                    {
                        new Supplier { Id = 1, Name = "Supplier 1" },
                        new Supplier { Id = 2, Name = "Supplier 2" },
                        new Supplier { Id = 3, Name = "Supplier 3" }
                    });

                    dbContext.SaveChanges();
                }
            }
        }
    }
}
