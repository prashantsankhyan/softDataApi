namespace softDataApi.Models
{
    public class CraditNoteForPurchaseRequest
    {
        public int? CreditNoteForPurchaseId { get; set; }
        public int? SaleInvoiceId { get; set; }
        public string? InvoiceHeading { get; set; }
        public int? InvoiceHeadingInt { get; set; }
        public int CompanyId { get; set; }
        public string? InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? ClaimDate { get; set; }
        public int AccountId { get; set; }
        public int? ShipTo { get; set; }
        public int? Transport { get; set; }
        public string? TransportNameManual { get; set; }

        public string? ShippingBillNo { get; set; }
        public string? GRNo { get; set; }
        public string? OrderNo { get; set; }
        public string? Vehicle { get; set; }
        public string? FormNo { get; set; }
        public string? Weight { get; set; }
        public string? CreditDays { get; set; }
        public string? PackingNo { get; set; }
        public string? DocuThru { get; set; }
        public string? Station { get; set; }
        public string? RGPNo { get; set; }
        public string? Dated { get; set; }
        public string? Freight { get; set; }

        public string? Packages { get; set; }
        public string? PvtMark { get; set; }
        public string? DueDate { get; set; }
        public string? EcomGSTIN { get; set; }
        public string? EwayNo { get; set; }
        public string? ShBNo { get; set; }
        public string? ShipDate { get; set; }
        public string? ShipPartNo { get; set; }
        public string? PortLoading { get; set; }
        public string? PortDischarge { get; set; }
        public string? FinalDestination { get; set; }
        public string? EntrBy { get; set; }
        public DateTime? EntryDate { get; set; }

        public decimal? SubTotal { get; set; }
        public decimal? RoundAndTotal { get; set; }
        public decimal? TaxableSale { get; set; }
        public decimal? CentralGst { get; set; }
        public decimal? LocalGst { get; set; }
        public decimal? Tcs { get; set; }
        public decimal? SwachBharat { get; set; }
        public int? OtherCharge { get; set; }
        public decimal? Value { get; set; }
        public int? OtherCharge1 { get; set; }
        public decimal? Value1 { get; set; }
        public decimal? ExtraAmount { get; set; }
        public int? InvoiceSeq { get; set; }

        public string? BillNo { get; set; }
        public string? BillDate { get; set; }
        public decimal? BillAmount { get; set; }
        public decimal? BillTaxAmount { get; set; }
        public string? BillFreight { get; set; }
        public decimal? Advance { get; set; }

        public List<CraditNoteForPurchaseDetailRequest>?
            CreditNoteForPurchaseDetails
        { get; set; }
    }

    public class CraditNoteForPurchaseDetailRequest
    {
        public int? SaleInvoiceDetailId { get; set; }
        public int? SaleInvoiceId { get; set; }
        public string? Barcode { get; set; }
        public int? ItemId { get; set; }
        public string? Remarks { get; set; }
        public string? HSN { get; set; }
        public string? ArtNo { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? Pack1 { get; set; }
        public string? Pack2 { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Rate { get; set; }
        public decimal? MRate { get; set; }
        public decimal? DiscPer { get; set; }
        public decimal? DiscAmt { get; set; }
        public int? TaxableValueId { get; set; }
        public int? AccountId { get; set; }
        public decimal? TaxPercent { get; set; }
        public decimal? RowTotal { get; set; }
        public decimal? taxTableRowSubTotal { get; set; }
        public int? Unit { get; set; }
        public decimal? IncludedRate { get; set; }
    }
    public class CraditNoteForPurchaseApiResponse
    {
        public int Success { get; set; }
        public string? Message { get; set; }
        public int? CreditNoteForPurchaseId { get; set; }
    }
 
public class CraditNoteForPurchaseByCompanyIdDto
    {
        public int CreditNoteForPurchaseId { get; set; }
        public int? SaleInvoiceId { get; set; }
        public string? InvoiceHeading { get; set; }
        public int? InvoiceHeadingInt { get; set; }
        public int? CompanyId { get; set; }
        public string? InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? ClaimDate { get; set; }
        public int? AccountId { get; set; }
        public int? ShipTo { get; set; }
        public string? AccountName { get; set; }
        public string? ShipToName { get; set; }

        public int? Transport { get; set; }
        public string? TransportNameManual { get; set; }
        public string? ShippingBillNo { get; set; }
        public string? GRNo { get; set; }
        public string? OrderNo { get; set; }
        public string? Vehicle { get; set; }
        public string? FormNo { get; set; }
        public string? Weight { get; set; }
        public string? CreditDays { get; set; }
        public string? PackingNo { get; set; }
        public string? DocuThru { get; set; }
        public string? Station { get; set; }
        public string? RGPNo { get; set; }
        public string? Dated { get; set; }
        public string? Freight { get; set; }
        public string? Packages { get; set; }
        public string? PvtMark { get; set; }
        public string? DueDate { get; set; }
        public string? EcomGSTIN { get; set; }
        public string? EwayNo { get; set; }
        public string? ShBNo { get; set; }
        public string? ShipDate { get; set; }
        public string? ShipPartNo { get; set; }
        public string? PortLoading { get; set; }
        public string? PortDischarge { get; set; }
        public string? FinalDestination { get; set; }
        public string? EntrBy { get; set; }
        public DateTime? EntryDate { get; set; }

        public decimal? SubTotal { get; set; }
        public decimal? RoundAndTotal { get; set; }
        public decimal? TaxableSale { get; set; }
        public decimal? CentralGst { get; set; }
        public decimal? LocalGst { get; set; }
        public decimal? Tcs { get; set; }
        public decimal? SwachBharat { get; set; }
        public int? OtherCharge { get; set; }
        public decimal? Value { get; set; }
        public int? OtherCharge1 { get; set; }
        public decimal? Value1 { get; set; }
        public decimal? ExtraAmount { get; set; }
        public int? InvoiceSeq { get; set; }

        public string? BillNo { get; set; }
        public string? BillDate { get; set; }
        public decimal? BillAmount { get; set; }
        public decimal? BillTaxAmount { get; set; }
        public string? BillFreight { get; set; }
        public decimal? Advance { get; set; }

        public int? CreditNoteForPurchaseDetailId { get; set; }
        public int? SaleInvoiceDetailId { get; set; }
        public int? DetailSaleInvoiceId { get; set; }
        public string? Barcode { get; set; }
        public int? ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? Remarks { get; set; }
        public string? HSN { get; set; }
        public string? ArtNo { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? Pack1 { get; set; }
        public string? Pack2 { get; set; }

        public decimal? Qty { get; set; }
        public decimal? Rate { get; set; }
        public decimal? MRate { get; set; }
        public decimal? DiscPer { get; set; }
        public decimal? DiscAmt { get; set; }
        public int? TaxableValueId { get; set; }
        public string? SalePurcAccountName { get; set; }
        public int? DetailAccountId { get; set; }
        public decimal? TaxPercent { get; set; }
        public decimal? RowTotal { get; set; }
        public decimal? taxTableRowSubTotal { get; set; }
        public int? Unit { get; set; }
        public decimal? IncludedRate { get; set; }
        public string? UnitName { get; set; }
    }



    public class CraditNoteForPurchaseResponseDto
    {
        public int CreditNoteForPurchaseId { get; set; }
        public int? SaleInvoiceId { get; set; }
        public string? InvoiceHeading { get; set; }
        public int? InvoiceHeadingInt { get; set; }
        public int? CompanyId { get; set; }
        public string? InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? ClaimDate { get; set; }
        public int? AccountId { get; set; }
        public int? ShipTo { get; set; }

        public string? AccountName { get; set; }
        public string? ShipToName { get; set; }
        public int? Transport { get; set; }
        public string? TransportNameManual { get; set; }

        public string? ShippingBillNo { get; set; }
        public string? GRNo { get; set; }
        public string? OrderNo { get; set; }
        public string? Vehicle { get; set; }
        public string? FormNo { get; set; }
        public string? Weight { get; set; }
        public string? CreditDays { get; set; }
        public string? PackingNo { get; set; }
        public string? DocuThru { get; set; }
        public string? Station { get; set; }
        public string? RGPNo { get; set; }
        public string? Dated { get; set; }
        public string? Freight { get; set; }
        public string? Packages { get; set; }
        public string? PvtMark { get; set; }
        public string? DueDate { get; set; }
        public string? EcomGSTIN { get; set; }
        public string? EwayNo { get; set; }
        public string? ShBNo { get; set; }
        public string? ShipDate { get; set; }
        public string? ShipPartNo { get; set; }
        public string? PortLoading { get; set; }
        public string? PortDischarge { get; set; }
        public string? FinalDestination { get; set; }
        public string? EntrBy { get; set; }
        public DateTime? EntryDate { get; set; }

        public decimal? SubTotal { get; set; }
        public decimal? RoundAndTotal { get; set; }
        public decimal? TaxableSale { get; set; }
        public decimal? CentralGst { get; set; }
        public decimal? LocalGst { get; set; }
        public decimal? Tcs { get; set; }
        public decimal? SwachBharat { get; set; }
        public int? OtherCharge { get; set; }
        public decimal? Value { get; set; }
        public int? OtherCharge1 { get; set; }
        public decimal? Value1 { get; set; }
        public decimal? ExtraAmount { get; set; }
        public int? InvoiceSeq { get; set; }

        public string? BillNo { get; set; }
        public string? BillDate { get; set; }
        public decimal? BillAmount { get; set; }
        public decimal? BillTaxAmount { get; set; }
        public string? BillFreight { get; set; }
        public decimal? Advance { get; set; }

        public List<CraditNoteForPurchaseDetailResponseDto> Details { get; set; } = new();
    }
    public class CraditNoteForPurchaseDetailResponseDto
    {
        public int? CreditNoteForPurchaseDetailId { get; set; }
        public int? SaleInvoiceDetailId { get; set; }
        public int? DetailSaleInvoiceId { get; set; }

        public string? Barcode { get; set; }
        public int? ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? Remarks { get; set; }
        public string? HSN { get; set; }
        public string? ArtNo { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? Pack1 { get; set; }
        public string? Pack2 { get; set; }

        public decimal? Qty { get; set; }
        public decimal? Rate { get; set; }
        public decimal? MRate { get; set; }
        public decimal? DiscPer { get; set; }
        public decimal? DiscAmt { get; set; }

        public int? TaxableValueId { get; set; }
        public string? SalePurcAccountName { get; set; }
        public int? DetailAccountId { get; set; }
        public decimal? TaxPercent { get; set; }
        public decimal? RowTotal { get; set; }
        public decimal? TaxTableRowSubTotal { get; set; }
        public int? Unit { get; set; }
        public decimal? IncludedRate { get; set; }
        public string? UnitName { get; set; }
    }
}
