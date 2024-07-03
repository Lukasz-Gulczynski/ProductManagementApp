using System.Collections.ObjectModel;

namespace ProductManagementApp.DAL.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Stock { get; set; }
        public List<byte[]> Images { get; set; }
        public bool IsFeatured { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }
}