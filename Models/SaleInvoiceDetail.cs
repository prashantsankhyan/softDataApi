using System;
using System.Collections.Generic;

namespace softDataApi.Models;

public partial class SaleInvoiceDetail
{
    public int SaleInvoiceDetailId { get; set; }

    public int SaleInvoiceId { get; set; }

    public string? Barcode { get; set; }

    public int ItemId { get; set; }

    public string? Remarks { get; set; }

    public string? Hsn { get; set; }

    public string? ArtNo { get; set; }

    public string? Size { get; set; }

    public string? Color { get; set; }

    public string? Pack1 { get; set; }

    public string? Pack2 { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Rate { get; set; }

    public decimal? Mrate { get; set; }

    public decimal? DiscPer { get; set; }

    public decimal? DiscAmt { get; set; }

    public int? TaxableValueId { get; set; }

    public int? AccountId { get; set; }

    public decimal? TaxPercent { get; set; }

    public decimal? RowTotal { get; set; }

    public decimal? TaxTableRowSubTotal { get; set; }
}
