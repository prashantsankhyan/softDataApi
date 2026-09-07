using System;
using System.Collections.Generic;

namespace softDataApi.Models;

public partial class TbItemMaster
{
    public int ItemId { get; set; }

    public int CompanyId { get; set; }

    public string? Description { get; set; }

    public string? ItemName { get; set; }


    public int? CategoryId { get; set; }

    public string? Unit { get; set; }

    public decimal? SaleRate { get; set; }

    public decimal? PurchaseRate { get; set; }

    public string? Hsn { get; set; }

    public string? ItemType { get; set; }

    public string? EnterBy { get; set; }

    public DateTime? EnteredOn { get; set; }

    public int? UnitInt { get; set; }

    public decimal? Discount { get; set; }

    public decimal? Total { get; set; }

    public decimal? PackingInUnit { get; set; }

    public decimal? Packing { get; set; }

    public string? BarCodeType { get; set; }

    public string? ItemBarCodeOrPartNo { get; set; }

    public decimal? MrpRate { get; set; }

    public decimal? OpeningStock { get; set; }

    public decimal? OpeningStockRate { get; set; }

    public decimal? OpeningStockValue { get; set; }

    public int? CgstSgstSale { get; set; }

    public int? IgstSaleName { get; set; }

    public int? CgstSgstPurchase { get; set; }

    public int? IgstPurchase { get; set; }

    public decimal? TaxRate { get; set; }

    public decimal? CessRate { get; set; }

    public decimal? CessQty { get; set; }

    public int? ItemGroupId { get; set; }
}


public class ItemMasterListDto
{
    // =====================================================
    // ITEM MASTER
    // =====================================================

    public int ItemId { get; set; }
    public int CompanyId { get; set; }
    public string? CompanyName { get; set; }


    // =====================================================
    // CATEGORY
    // =====================================================

    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? CategoryType { get; set; }


    // =====================================================
    // ITEM DETAILS
    // =====================================================

    public string? Description { get; set; }
    public string? ItemName { get; set; }
    public string? Unit { get; set; }
    public int? UnitInt { get; set; }

    public decimal? Discount { get; set; }
    public decimal? Total { get; set; }
    public decimal? SaleRate { get; set; }

    // Item master purchase rate
    public decimal? PurchaseRate { get; set; }

    // Latest purchase rate from PurchaseInvoiceDetail
    public decimal? LastPurchaseRate { get; set; }

    // All purchase rates
    // Example:
    // 120, 125, 110, 100
    public string? PurchaseRates { get; set; }

    // Purchase history
    // Example:
    // 05-09-2026 : 120 |
    // 02-09-2026 : 125 |
    // 28-08-2026 : 110
    public string? PurchaseHistory { get; set; }

    public decimal? MrpRate { get; set; }

    public string? Hsn { get; set; }
    public string? ItemType { get; set; }


    // =====================================================
    // OPENING STOCK
    // =====================================================

    public decimal? MasterOpeningQty { get; set; }

    public decimal? OpeningStock { get; set; }
    public decimal? OpeningStockRate { get; set; }
    public decimal? OpeningStockValue { get; set; }


    // =====================================================
    // AUDIT
    // =====================================================

    public string? EnterBy { get; set; }
    public DateTime? EnteredOn { get; set; }


    // =====================================================
    // ITEM EXTRA FIELDS
    // =====================================================

    public decimal? PackingInUnit { get; set; }
    public decimal? Packing { get; set; }

    public string? BarCodeType { get; set; }
    public string? ItemBarCodeOrPartNo { get; set; }

    public int? CgstSgstSale { get; set; }
    public int? IgstSaleName { get; set; }
    public int? CgstSgstPurchase { get; set; }
    public int? IgstPurchase { get; set; }

    public decimal? TaxRate { get; set; }
    public decimal? CessRate { get; set; }
    public decimal? CessQty { get; set; }


    // =====================================================
    // UNIT DETAILS
    // =====================================================

    public int? UnitId { get; set; }
    public string? UnitName { get; set; }
    public string? Quantity { get; set; }
    public string? Decimal { get; set; }


    // =====================================================
    // ITEM GROUP
    // =====================================================

    public int? ItemGroupId { get; set; }
    public string? ItemGroupName { get; set; }
    public int? ItemGroup { get; set; }
    public string? ParentGroupName { get; set; }


    // =====================================================
    // STOCK CALCULATION
    // =====================================================

    public decimal PreviousPurchaseQty { get; set; }
    public decimal PreviousSaleQty { get; set; }

    public decimal OpeningQty { get; set; }

    public decimal PurchaseQty { get; set; }
    public decimal SaleQty { get; set; }

    public decimal NetQty { get; set; }

    public decimal ClosingQty { get; set; }

    public decimal PurchaseValue { get; set; }
    public decimal SaleValue { get; set; }
}

public class ItemWiseStockTransactionDto
{
    public string? TransactionType { get; set; }

    public int ItemId { get; set; }

    public string? ItemName { get; set; }

    public int InvoiceId { get; set; }

    public string? InvoiceNo { get; set; }

    public DateTime? InvoiceDate { get; set; }

    public decimal Qty { get; set; }

    public decimal Rate { get; set; }

    public decimal Amount { get; set; }
}

public class ItemWiseStockDto
{
    public int ItemId { get; set; }
    public string? ItemName { get; set; }
    public string? Description { get; set; }
    public string? Unit { get; set; }
    public string? Hsn { get; set; }

    public decimal MasterOpeningQty { get; set; }
    public decimal PreviousPurchaseQty { get; set; }
    public decimal PreviousSaleQty { get; set; }
    public decimal OpeningQty { get; set; }

    public decimal PurchaseQty { get; set; }
    public decimal SaleQty { get; set; }
    public decimal NetQty { get; set; }
    public decimal ClosingQty { get; set; }

    public decimal PurchaseValue { get; set; }
    public decimal SaleValue { get; set; }
}