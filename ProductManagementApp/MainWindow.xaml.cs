using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using ProductManagementApp.Enums;
using ProductManagementApp.Interfaces;
using ProductManagementApp.Mappers;
using ProductManagementApp.Services;
using ProductManagementApp.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace ProductManagementApp
{
    public partial class MainWindow : Window
    {
        private readonly ProductService _productService;
        private readonly ObservableCollection<string> _selectedFiles = new ObservableCollection<string>();
        private readonly ObservableCollection<SelectedFileItemViewModel> _fileSupplierMappings = new ObservableCollection<SelectedFileItemViewModel>();

        public MainWindow()
        {
            InitializeComponent();

            var serviceProvider = App.ServiceProvider;

            DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>();
            _productService = serviceProvider.GetRequiredService<ProductService>();

            var viewModel = DataContext as MainWindowViewModel;

            if (viewModel != null)
            {
                viewModel.PropertyChanged += ViewModel_PropertyChanged;

                viewModel.FileSupplierMappings = _fileSupplierMappings;
            }

            SelectedFilesListBox.ItemsSource = _fileSupplierMappings;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainWindowViewModel.Products))
            {
                UpdateListBox();
            }
        }

        private void UpdateListBox()
        {
            var viewModel = DataContext as MainWindowViewModel;

            if (viewModel?.Products == null) return;

            ProductsListBox.ItemsSource = null;
            ProductsListBox.ItemsSource = viewModel.Products;
        }

        private void ChooseXmlFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
                FilterIndex = 1,
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
                    _fileSupplierMappings.Add(new SelectedFileItemViewModel { FileName = Path.GetFileName(filePath), FullPath = filePath, SupplierIds = new ObservableCollection<int>() });
                }
            }
        }

        private void ShowProducts_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;

            if (viewModel != null)
            {
                viewModel.Products.Clear();

                Dictionary<int, ProductViewModel> mergedProducts = new Dictionary<int, ProductViewModel>();

                foreach (var fileMapping in _fileSupplierMappings)
                {
                    string filePath = fileMapping.FullPath;
                    if (!File.Exists(filePath))
                    {
                        MessageBox.Show($"Error while processing file {filePath}. Could not find part of the path {filePath}", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        continue;
                    }

                    SupplierType supplierType = (SupplierType)fileMapping.SelectedSupplierId;

                    var xmlProductLoader = App.ServiceProvider.GetRequiredService<XmlProductLoader>();

                    List<ProductViewModel> products = xmlProductLoader.LoadProductsFromXml(filePath, supplierType);

                    IProductMapper mapper = GetMapperForSupplier(xmlProductLoader, supplierType);

                    if (mapper == null)
                    {
                        MessageBox.Show($"Mapper not found for SupplierType {supplierType}.", "Mapper Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        continue;
                    }

                    // Merge products
                    foreach (var product in products)
                    {
                        if (mergedProducts.ContainsKey(product.ProductId))
                        {
                            var existingProduct = mergedProducts[product.ProductId];
                            mapper.MergeProduct(existingProduct, product);
                        }
                        else
                        {
                            mergedProducts.Add(product.ProductId, product);
                        }
                    }
                }

                foreach (var mergedProduct in mergedProducts.Values)
                {
                    viewModel.Products.Add(mergedProduct);
                }
            }
        }

        private IProductMapper GetMapperForSupplier(XmlProductLoader xmlProductLoader, SupplierType supplierType)
        {
            switch (supplierType)
            {
                case SupplierType.Supplier1:
                    return new Supplier1ProductMapper();
                case SupplierType.Supplier2:
                    return new Supplier2ProductMapper();
                case SupplierType.Supplier3:
                    return new Supplier3ProductMapper();
                default:
                    return null;
            }
        }

        private void SaveFeaturedProducts_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            if (viewModel != null && viewModel.Products != null)
            {
                _productService.SaveFeaturedProducts(viewModel.Products);
                MessageBox.Show("Featured products saved successfully.");
            }
        }
    }
}
