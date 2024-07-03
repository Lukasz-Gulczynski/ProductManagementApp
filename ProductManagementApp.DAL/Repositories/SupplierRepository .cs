using ProductManagement.DAL;
using ProductManagementApp.DAL.Interfaces;
using ProductManagementApp.DAL.Models;
using System.Collections.ObjectModel;

namespace ProductManagementApp.DAL.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ProductManagementAppContext _context;

        public SupplierRepository(ProductManagementAppContext context)
        {
            _context = context;
        }

        public ObservableCollection<Supplier> GetAllSuppliers()
        {
            return new ObservableCollection<Supplier>(_context.Suppliers.ToList());
        }
    }
}
