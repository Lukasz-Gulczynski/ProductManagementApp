using ProductManagementApp.ViewModels;
using System.Xml.Linq;

namespace ProductManagementApp.Interfaces
{
    public interface IProductMapper
    {
        ProductViewModel MapFromXml(XElement productElement);
        void MergeProduct(ProductViewModel existingProduct, ProductViewModel newProduct);
    }
}
