using ProductManagement.DAL;
using ProductManagementApp.DAL.Models;
using ProductManagementApp.Helpers;
using ProductManagementApp.ViewModels;

namespace ProductManagementApp.Services
{
    public class ProductService
    {
        private readonly ProductManagementAppContext _dbContext;

        public ProductService(ProductManagementAppContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SaveFeaturedProducts(IEnumerable<ProductViewModel> products)
        {
            foreach (var productViewModel in products)
            {
                try
                {
                    var existingProduct = _dbContext.Products.FirstOrDefault(p => p.ProductId == productViewModel.ProductId);

                    if (existingProduct != null)
                    {
                        existingProduct.IsFeatured = productViewModel.IsFeatured;
                    }
                    else
                    {
                        var newProduct = new Product
                        {
                            ProductId = productViewModel.ProductId,
                            Name = productViewModel.Name,
                            Description = productViewModel.Description,
                            Stock = productViewModel.Stock,
                            IsFeatured = productViewModel.IsFeatured,
                            SupplierId = productViewModel.SupplierId,
                            Images = productViewModel.Images?.Select(ImageHelper.BitmapImageToByteArray).ToList(),
                        };

                        _dbContext.Products.Add(newProduct);
                    }

                    _dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while saving product with ProductId {productViewModel.ProductId}: {ex.Message}");
                    throw;
                }
            }
        }
    }
}