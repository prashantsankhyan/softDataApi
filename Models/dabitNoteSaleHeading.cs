using Microsoft.EntityFrameworkCore;

namespace softDataApi.Models
{
    public class dabitNoteSaleHeading
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

    public class NextDabitNoteNoDto
    {
        public string? NextDabitNoteNo { get; set; }
    }


    //public class NextInvoiceNoDto
    //{
    //    public string? NextInvoiceNo { get; set; }
    //}
}
