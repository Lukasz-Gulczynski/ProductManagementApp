using ProductManagementApp.Interfaces;
using ProductManagementApp.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace ProductManagementApp.Mappers
{
    public class Supplier3ProductMapper : IProductMapper
    {
        public ProductViewModel MapFromXml(XElement productElement)
        {
            if (productElement == null)
                return null;

            return new ProductViewModel
            {
                ProductId = GetElementValueAsInt(productElement, "id"),
                Name = GetElementValue(productElement, "nazwa"),
                Description = GetElementValue(productElement, "dlugi_opis"),
                Stock = null,
                IsFeatured = false,
                Images = GetBitmapImages(GetImageUrls(productElement))
            };
        }

        private int GetElementValueAsInt(XElement element, string nodeName)
        {
            var node = element.Element(nodeName);
            if (node != null && int.TryParse(node.Value, out int value))
                return value;
            return 0;
        }

        private string GetElementValue(XElement element, string nodeName)
        {
            var node = element.Element(nodeName);
            return node?.Value ?? string.Empty;
        }

        private List<string> GetImageUrls(XElement element)
        {
            return element.Descendants("zdjecie")
                          .Select(photo => photo.Attribute("url")?.Value)
                          .Where(url => !string.IsNullOrEmpty(url))
                          .ToList();
        }

        private ObservableCollection<BitmapImage> GetBitmapImages(List<string> imageUrls)
        {
            var bitmapImages = new ObservableCollection<BitmapImage>();

            foreach (var url in imageUrls)
            {
                try
                {
                    var bitmapImage = new BitmapImage(new System.Uri(url));
                    bitmapImages.Add(bitmapImage);
                }
                catch (System.Exception)
                {
                }
            }

            return bitmapImages;
        }

        public void MergeProduct(ProductViewModel existingProduct, ProductViewModel newProduct)
        {
            throw new NotImplementedException();
        }
    }
}
