using ProductManagementApp.Interfaces;
using ProductManagementApp.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace ProductManagementApp.Mappers
{
    public class Supplier2ProductMapper : IProductMapper
    {
        public ProductViewModel MapFromXml(XElement productElement)
        {
            if (productElement == null)
                return null;

            return new ProductViewModel
            {
                ProductId = GetProductId(productElement),
                Name = GetElementValue(productElement, "sku"),
                Description = GetElementValue(productElement, "desc"),
                Stock = GetElementValueAsInt(productElement, "qty"),
                IsFeatured = false,
                Images = GetBitmapImages(GetImageUrls(productElement))
            };
        }

        private int GetProductId(XElement element)
        {
            var idElement = element.Element("id");
            if (idElement != null && int.TryParse(idElement.Value, out int productId))
                return productId;
            return 0;
        }

        private string GetElementValue(XElement element, string nodeName)
        {
            var node = element.Element(nodeName);
            return node?.Value.Trim() ?? string.Empty;
        }

        private int GetElementValueAsInt(XElement element, string nodeName)
        {
            var node = element.Element(nodeName);
            if (node != null && int.TryParse(node.Value, out int value))
                return value;
            return 0;
        }

        private List<string> GetImageUrls(XElement element)
        {
            return element.Descendants("photo")
                          .Select(photo => photo.Value.Trim())
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
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(url, UriKind.Absolute);
                    bitmapImage.EndInit();
                    bitmapImages.Add(bitmapImage);
                }
                catch (Exception)
                {
                    MessageBox.Show($"Error while converting url to bitmapImage");
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