namespace softDataApi.Models
{
    public class creditNotePurchaseHeading
    {
        public int Id { get; set; }

        public string TypeOfSale { get; set; } = null!;

        public string? Prefix { get; set; }

        public string? Suffix { get; set; }

        public string? TaxOnSaleType { get; set; }

        public string? NumberStartFrom { get; set; }

        public int? CompanyId { get; set; }
        public string? Permission { get; set; }
    }
    public class NextCreditNoteNoDto
    {
        public string? NextCreditNoteNo { get; set; }
    }

}
