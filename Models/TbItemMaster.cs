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
    public int ItemId { get; set; }
    public int CompanyId { get; set; }
    public string? CompanyName { get; set; }

    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? CategoryType { get; set; }


    public string? Description { get; set; }
    public string? ItemName { get; set; }
    public string? Unit { get; set; }
    public int? UnitInt { get; set; }

    public decimal? Discount { get; set; }
    public decimal? Total { get; set; }
    public decimal? SaleRate { get; set; }

    public decimal? PurchaseRate { get; set; }
    public decimal? MrpRate { get; set; }

    public string? Hsn { get; set; }
    public string? ItemType { get; set; }

    public decimal? OpeningStock { get; set; }
    public decimal? OpeningStockRate { get; set; }
    public decimal? OpeningStockValue { get; set; }

    public string? EnterBy { get; set; }
    public DateTime? EnteredOn { get; set; }

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

    // Unit details
    public int? UnitId { get; set; }
    public string? UnitName { get; set; }
    public string? Quantity { get; set; }
    public string? Decimal { get; set; }

    // Item Group details
    public int? ItemGroupId { get; set; }
    public string? ItemGroupName { get; set; }
    public int? ItemGroup { get; set; }
    public string? ParentGroupName { get; set; }
}
