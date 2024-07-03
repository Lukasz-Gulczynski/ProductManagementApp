using ProductManagementApp.ViewModels;

namespace ProductManagementApp.Interfaces
{
    public interface IProductLoader
    {
        List<ProductViewModel> LoadProductsFromXml(string filePath);
    }
}