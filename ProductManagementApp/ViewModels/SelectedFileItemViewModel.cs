using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProductManagementApp.ViewModels
{
    public class SelectedFileItemViewModel : INotifyPropertyChanged
    {
        private int _selectedSupplierId;

        public string FileName { get; set; }
        public string FullPath { get; set; }

        public ObservableCollection<int> SupplierIds { get; set; }

        public int SelectedSupplierId
        {
            get => _selectedSupplierId;
            set
            {
                if (_selectedSupplierId != value)
                {
                    _selectedSupplierId = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
