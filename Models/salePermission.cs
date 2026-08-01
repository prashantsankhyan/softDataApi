namespace softDataApi.Models
{
    public class salePermission
    {
        public int CompanyId { get; set; }
        public bool? ExcludedOrIncluded { get; set; }
    }

    public class SaleAddPermissionResponse
    {
        public bool value { get; set; }
    }
}
