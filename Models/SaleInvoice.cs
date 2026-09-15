using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace softDataApi.Models;

[Keyless]
public partial class SaleInvoice
{
    public int SaleInvoiceId { get; set; }

    public string? InvoiceHeading { get; set; }

    public int CompanyId { get; set; }

    public string? InvoiceNo { get; set; }

    public DateTime? InvoiceDate { get; set; }

    public DateTime? ClaimDate { get; set; }

    public int AccountId { get; set; }

    public int? ShipTo { get; set; }

    

    public int? Transport { get; set; }


    public string? TransportNameManual { get; set; }

    

    public string? ShippingBillNo { get; set; }

    public string? Grno { get; set; }

    public string? OrderNo { get; set; }

    public string? Vehicle { get; set; }

    public string? FormNo { get; set; }

    public string? Weight { get; set; }

    public string? CreditDays { get; set; }

    public string? PackingNo { get; set; }

    public string? DocuThru { get; set; }

    public string? Station { get; set; }

    public string? Rgpno { get; set; }

    public string? Dated { get; set; }

    public string? Freight { get; set; }

    public string? Packages { get; set; }

    public string? PvtMark { get; set; }

    public string? DueDate { get; set; }

    public string? EcomGstin { get; set; }

    public string? EwayNo { get; set; }

    public string? ShBno { get; set; }

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

    public int? InvoiceHeadingInt { get; set; }

    public decimal? ExtraAmount { get; set; }
}



public partial class SaleInvoiceRequest
{
    public int? SaleInvoiceId { get; set; }
    public string? InvoiceHeading { get; set; }
    public int? InvoiceHeadingInt { get; set; }
    public int CompanyId { get; set; }

    public int? ShipTo { get; set; }
    public string? InvoiceNo { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime? ClaimDate { get; set; }
    public int AccountId { get; set; }
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

    public decimal? SubTotal { get; set; }
    public decimal? RoundAndTotal { get; set; }
    public decimal? TaxableSale { get; set; }
    public decimal? CentralGst { get; set; }
    public decimal? LocalGst { get; set; }
    public decimal? Tcs { get; set; }
    public decimal? SwachBharat { get; set; }
    public decimal? Value { get; set; }
    public int OtherCharge { get; set; }
    public decimal? Value1 { get; set; }
    public int OtherCharge1 { get; set; }
    public decimal ExtraAmount { get; set; }

    // JSON string of SaleInvoiceDetail rows
    public List<SaleInvoiceDetailRequest> SaleInvoiceDetails { get; set; } = new();

}
public class SaleInvoiceDetailRequest
{
    public string? Barcode { get; set; }
    public int ItemId { get; set; }
    public string? Remarks { get; set; }
    public string? HSN { get; set; }   // ✅ MUST be HSN (not Hsn)

    public string? ArtNo { get; set; }
    public string? Size { get; set; }
    public string? Color { get; set; }
    public string? Pack1 { get; set; }
    public string? Pack2 { get; set; }
    public decimal? Qty { get; set; }
    public decimal Rate { get; set; }
    public decimal MRate { get; set; }
    public decimal DiscPer { get; set; }
    public decimal DiscAmt { get; set; }
    public int TaxableValueId { get; set; }
    public int AccountId { get; set; }
    public decimal TaxPercent { get; set; }
    public decimal SubTaxPercent { get; set; }
    public decimal RowTotal { get; set; }
    public decimal taxTableRowSubTotal { get; set; }

    public int? Unit { get; set; }
}

public class ApiResponse
{
    public int Success { get; set; }
    public string? Message { get; set; }
    public int SaleInvoiceId { get; set; }
}


public class SaleInvoiceDto
{
    public int SaleInvoiceId { get; set; }
    public string? InvoiceHeading { get; set; }
    public int? InvoiceHeadingInt { get; set; }
    public int CompanyId { get; set; }
    public string? InvoiceNo { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime? ClaimDate { get; set; }

    public int AccountId { get; set; }

    public int ShipTo { get; set; }
    public string? ShipToName { get; set; }
    public string? AccountName { get; set; }

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

    // Totals
    public decimal? SubTotal { get; set; }
    public decimal? RoundAndTotal { get; set; }
    public decimal? TaxableSale { get; set; }
    public decimal? CentralGst { get; set; }
    public decimal? LocalGst { get; set; }
    public decimal? Tcs { get; set; }
    public decimal? SwachBharat { get; set; }
    public int OtherCharge { get; set; }
    public decimal? Value { get; set; }
    public int OtherCharge1 { get; set; }
    public decimal? Value1 { get; set; }
    public decimal ExtraAmount { get; set; }

    // SaleHeading
    public string? TypeOfSale { get; set; }
    public string? Prefix { get; set; }
    public string? Suffix { get; set; }
    public string? TaxOnSaleType { get; set; }
    public int? NumberStartFrom { get; set; }

    public List<SaleInvoiceDetailDto> Details { get; set; } = new();
}
public class SaleInvoiceDetailDto
{
    public int SaleInvoiceDetailId { get; set; }
    public string? Barcode { get; set; }
    public int ItemId { get; set; }
    public string? ItemName { get; set; }
    public string? Remarks { get; set; }
    public string? HSN { get; set; }
    public string? ArtNo { get; set; }
    public string? Size { get; set; }
    public string? Color { get; set; }
    public string? Pack1 { get; set; }
    public string? Pack2 { get; set; }
    public decimal Qty { get; set; }
    public decimal Rate { get; set; }
    public decimal MRate { get; set; }
    public decimal DiscPer { get; set; }
    public decimal DiscAmt { get; set; }
    public int TaxableValueId { get; set; }
    public int DetailAccountId { get; set; }
    public decimal TaxPercent { get; set; }
    public decimal RowTotal { get; set; }
    public decimal TaxTableRowSubTotal { get; set; }

    public int? Unit { get; set; }
    public string? UnitName { get; set; }
    public string? SalePurcAccountName { get; set; }
}



public class SaleInvoiceHeaderPdf
{
    public int SaleInvoiceId { get; set; }
    public string? InvoiceHeading { get; set; }
    public int CompanyId { get; set; }
    public string? InvoiceNo { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public int AccountId { get; set; }
    public string? accountName { get; set; }

    public string? companyName { get; set; }

    public string? companyPhone { get; set; }

    public string? cityName { get; set; }
    public string? stateName { get; set; }

    public string? stateCode { get; set; }
    public string? OrderNo { get; set; }

    public string? TransportName { get; set; }
    public string? TransportPhone { get; set; }
    public string? TransportGSTNo { get; set; }
    public string? TransportNameManual { get; set; }
    public string? ShippingBillNo { get; set; }
    public string? GRNo { get; set; }

    public decimal? swachBharat { get; set; }
    public decimal? tcs { get; set; }
    public decimal? localGst { get; set; }
    public decimal? centralGst { get; set; }
    public int? ShipToAccountId { get; set; }

    public string? PartyName { get; set; }
    public string? PartyAddress { get; set; }
    public string? PartyPhone { get; set; }
    public string? PartyZipCode { get; set; }
    public string? PartyEmail { get; set; }
 
    public string? PartyGst { get; set; }
    public string? PartyPan { get; set; }
    public string? PartyAdharNo { get; set; }
    public string? PartyContectName { get; set; }
    public string? PartyContectNo { get; set; }
    public string? PartyBankName { get; set; }

    public string?  PartyIfscCode { get; set; }


    public int? AccountCityId { get; set; }
    public string? PartyCity { get; set; }
    public string? PartyState { get; set; }
    public string? PartyStateCode { get; set; }

    public string? ShipToPartyName { get; set; }
    public string? ShipToAddress { get; set; }
    public string? ShipToZipCode { get; set; }

    public string? ShipToGst { get; set; }
    public string? ShipToPan { get; set; }
    public string? ShipToAdharNo { get; set; }
    public string? ShipToContectName { get; set; }

    public string? ShipToContectNo { get; set; }
    public string? ShipToBankName { get; set; }
    public string? ShipToIfscCode { get; set; }
   
    public int? ShipToCityId { get; set; }
    public string? ShipToCityName { get; set; }
    public string? ShipToStateName { get; set; }
    public string? ShipToStateCode { get; set; }



    public decimal? SubTotal { get; set; }
    public decimal? RoundAndTotal { get; set; }

    public string? ShipToName { get; set; }

    public string? ShipToPhone { get; set; }
    public decimal? Value { get; set; }

    public string? OtherChargeName { get; set; }
    public decimal? Value1 { get; set; }

    public string? OtherCharge1Name { get; set; }
}

public class SaleInvoiceDetailDtoPdf
{
    public int SaleInvoiceDetailId { get; set; }
    public string? Barcode { get; set; }
    public int ItemId { get; set; }
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
    public int? DetailAccountId { get; set; }
    public decimal? TaxPercent { get; set; }
    public decimal? RowTotal { get; set; }
    public decimal? taxTableRowSubTotal { get; set; }
    public decimal? gstApplicabeCentralRate { get; set; }
    public decimal? gstApplicabeLocalRate { get; set; }
    public decimal? tcsApplicabeRate { get; set; }
    public decimal? swachBhartApplicableRate { get; set; }

    public string? ShipToName { get; set; }
    public string? ShipToPhone { get; set; }
    public decimal? Value { get; set; }

    public string? OtherChargeName { get; set; }
    public decimal? Value1 { get; set; }

    public string? OtherCharge1Name { get; set; }
}
