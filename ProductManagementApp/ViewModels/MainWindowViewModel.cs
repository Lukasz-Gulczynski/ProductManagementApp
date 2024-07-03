using ProductManagementApp.DAL;
using ProductManagementApp.DAL.Models;
using ProductManagementApp.DAL.Repositories;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProductManagementApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ISupplierRepository _supplierRepository;
        private Supplier _selectedSupplier;
        private ObservableCollection<Supplier> _suppliers; // Dodane pole _suppliers

        public ObservableCollection<Supplier> Suppliers
        {
            get => _suppliers;
            set
            {
                _suppliers = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ProductViewModel> _products;
        public ObservableCollection<ProductViewModel> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SelectedFileItemViewModel> _fileSupplierMappings;
        public ObservableCollection<SelectedFileItemViewModel> FileSupplierMappings
        {
            get => _fileSupplierMappings;
            set
            {
                _fileSupplierMappings = value;
                OnPropertyChanged();
            }
        }

        public Supplier SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                _selectedSupplier = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel(IServiceProvider serviceProvider, ISupplierRepository supplierRepository)
        {
            _serviceProvider = serviceProvider;
            _supplierRepository = supplierRepository;
            LoadSuppliers();
            Products = new ObservableCollection<ProductViewModel>();
            LoadFileSupplierMappings();
        }

        private void LoadFileSupplierMappings()
        {
            FileSupplierMappings = new ObservableCollection<SelectedFileItemViewModel>
            {
                new SelectedFileItemViewModel { FileName = "File1.xml", SupplierIds = new ObservableCollection<int> { 1, 2, 3 } },
                new SelectedFileItemViewModel { FileName = "File2.xml", SupplierIds = new ObservableCollection<int> { 2, 3 } },
                new SelectedFileItemViewModel { FileName = "File3.xml", SupplierIds = new ObservableCollection<int> { 1, 3 } }
            };
        }

        private void LoadSuppliers()
        {
            Suppliers = _supplierRepository.GetAllSuppliers();
        }
    }
}
