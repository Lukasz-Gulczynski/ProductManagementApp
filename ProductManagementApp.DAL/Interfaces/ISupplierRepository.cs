using ProductManagementApp.DAL.Models;
using System.Collections.ObjectModel;

namespace ProductManagementApp.DAL.Interfaces
{
    public interface ISupplierRepository
    {
        ObservableCollection<Supplier> GetAllSuppliers();
    }
}
