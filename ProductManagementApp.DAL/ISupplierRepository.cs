using ProductManagementApp.DAL.Models;
using System.Collections.ObjectModel;

namespace ProductManagementApp.DAL
{
    public interface ISupplierRepository
    {
        ObservableCollection<Supplier> GetAllSuppliers();
    }
}
