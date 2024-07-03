using ProductManagementApp.Enums;

namespace ProductManagementApp.Helpers
{
    public static class SupplierTypeHelper
    {
        public static string GetSupplierName(SupplierType supplierType)
        {
            switch (supplierType)
            {
                case SupplierType.Supplier1:
                    return "Supplier 1";
                case SupplierType.Supplier2:
                    return "Supplier 2";
                case SupplierType.Supplier3:
                    return "Supplier 3";
                default:
                    return "Unknown Supplier";
            }
        }
    }
}
