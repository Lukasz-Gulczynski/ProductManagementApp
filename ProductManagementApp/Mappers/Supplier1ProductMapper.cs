using ProductManagementApp.Interfaces;
using ProductManagementApp.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace ProductManagementApp.Mappers
{
    public class Supplier1ProductMapper : IProductMapper
    {
        public ProductViewModel MapFromXml(XElement productElement)
        {
            if (productElement == null)
                return null;

            return new ProductViewModel
            {
                ProductId = GetProductId(productElement),
                Name = GetLocalizedValue(productElement, "name", "pol"),
                Description = GetLocalizedValue(productElement, "long_desc", "pol"),
                Stock = GetStockQuantity(productElement),
                IsFeatured = false,
                Images = GetBitmapImages(GetImages(productElement)),
            };
        }

        private int GetProductId(XElement element)
        {
            var idAttribute = element.Attribute("id");
            if (idAttribute != null && int.TryParse(idAttribute.Value, out int productId))
                return productId;
            return 0;
        }

        private string GetLocalizedValue(XElement element, string nodeName, string language)
        {
            XElement localizedElement = element.DescendantsAndSelf()
                .FirstOrDefault(n => n.Name.LocalName == nodeName &&
                                     n.Attribute(XNamespace.Xml + "lang")?.Value == language);

            return localizedElement?.Value ?? string.Empty;
        }

        private int GetStockQuantity(XElement element)
        {
            var stockElement = element.Descendants("stock").FirstOrDefault();
            if (stockElement != null && stockElement.Attribute("quantity") != null)
            {
                if (int.TryParse(stockElement.Attribute("quantity").Value, out int quantity))
                    return quantity;
            }
            return 0;
        }

        private List<string> GetImages(XElement element)
        {
            var imageElements = element.Descendants("image")
                                       .Select(img => img.Attribute("url")?.Value)
                                       .Where(url => !string.IsNullOrEmpty(url))
                                       .ToList();
            return imageElements ?? new List<string>();
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
                    // Handle exceptions
                }
            }

            return bitmapImages;
        }

        public void MergeProduct(ProductViewModel existingProduct, ProductViewModel newProduct)
        {
            if (existingProduct.ProductId == newProduct.ProductId)
            {
                if (string.IsNullOrWhiteSpace(existingProduct.Name) && !string.IsNullOrWhiteSpace(newProduct.Name))
                {
                    existingProduct.Name = newProduct.Name;
                }

                if (string.IsNullOrWhiteSpace(existingProduct.Description) && !string.IsNullOrWhiteSpace(newProduct.Description))
                {
                    existingProduct.Description = newProduct.Description;
                }

                foreach (var image in newProduct.Images)
                {
                    if (!existingProduct.Images.Contains(image))
                    {
                        existingProduct.Images.Add(image);
                    }
                }
            }
        }
    }
}
