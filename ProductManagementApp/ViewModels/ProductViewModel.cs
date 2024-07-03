using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace ProductManagementApp.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        private int _productId;
        public int ProductId
        {
            get { return _productId; }
            set
            {
                _productId = value;
                OnPropertyChanged();
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private int? _stock;
        public int? Stock
        {
            get { return _stock; }
            set
            {
                _stock = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<BitmapImage> _images;
        public ObservableCollection<BitmapImage> Images
        {
            get { return _images; }
            set
            {
                _images = value;
                OnPropertyChanged();
            }
        }

        private bool _isFeatured;
        public bool IsFeatured
        {
            get { return _isFeatured; }
            set
            {
                _isFeatured = value;
                OnPropertyChanged();
            }
        }

        private string _supplierName;
        public string SupplierName
        {
            get { return _supplierName; }
            set
            {
                _supplierName = value;
                OnPropertyChanged();
            }
        }

        private int _supplierId;
        public int SupplierId
        {
            get { return _supplierId; }
            set
            {
                _supplierId = value;
                OnPropertyChanged();
            }
        }
    }
}
