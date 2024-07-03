using ProductManagement.DAL;
using ProductManagementApp.DAL.Models;
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
                        Images = productViewModel.Images.Select(ImageHelper.BitmapImageToByteArray).ToList(),
                        IsFeatured = productViewModel.IsFeatured,
                        SupplierId = productViewModel.SupplierId,                       
                    };

                    _dbContext.Products.Add(newProduct);
                }
            }

            _dbContext.SaveChanges();
        }
    }
}
