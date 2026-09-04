using Microsoft.EntityFrameworkCore;

namespace softDataApi.Models
{
    public class purchaseHeading
    {
        public int Id { get; set; }

        public string TypeOfPurchase { get; set; } = null!;

        public string? Prefix { get; set; }

        public string? Suffix { get; set; }

        public string? TaxOnPurchaseType { get; set; }

        public string? NumberStartFrom { get; set; }

        public int? CompanyId { get; set; }
        public string? Permission { get; set; }
    }

}


public class purchaseDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string? TypeOfPurchase { get; set; }
    public string? Prefix { get; set; }
    public string? Suffix { get; set; }
    public string? TaxOnPurchaseType { get; set; }
    public string? NumberStartFrom { get; set; }

    public string? Permission { get; set; }
}
[Keyless]
public class purchaseById
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string? TypeOfPurchase { get; set; }
    public string? Prefix { get; set; }
    public string? Suffix { get; set; }
    public string? TaxOnPurchaseType { get; set; }
    public string? NumberStartFrom { get; set; }

    public string? Permission { get; set; }
}


public class NextInvoiceHeadingNoDto
{
    public string? NextInvoiceNo { get; set; }
}


