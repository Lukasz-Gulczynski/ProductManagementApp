using ProductManagementApp.Interfaces;
using ProductManagementApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using ProductManagementApp.Enums;
using ProductManagementApp.Helpers;

namespace ProductManagementApp
{
    public class XmlProductLoader
    {
        private readonly IDictionary<SupplierType, IProductMapper> _mappers;

        public XmlProductLoader(IDictionary<SupplierType, IProductMapper> mappers)
        {
            _mappers = mappers ?? throw new ArgumentNullException(nameof(mappers));
        }

        public List<ProductViewModel> LoadProductsFromXml(string filePath, SupplierType supplierType)
        {
            List<ProductViewModel> products = new List<ProductViewModel>();
            string supplierName;

            try
            {
                XDocument doc = XDocument.Load(filePath);

                if (!_mappers.TryGetValue(supplierType, out IProductMapper mapper))
                {
                    MessageBox.Show($"Mapper for {supplierType} not found.");
                    return products;
                }

                foreach (XElement productElement in doc.Root.Elements())
                {
                    ProductViewModel newProduct = mapper.MapFromXml(productElement);
                    newProduct.SupplierName = SupplierTypeHelper.GetSupplierName(supplierType);
                    products.Add(newProduct);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while processing file {filePath}: {ex.Message}");
            }

            return products;
        }
    }
}
