using System;
using System.Collections.Generic;

namespace softDataApi.Models;

public partial class ScreenManagement
{
    public int CompanyId { get; set; }
    public int ScreenId { get; set; }

    // Sale
    public bool? RemarksSale { get; set; }
    public bool? HSNSale { get; set; }
    public bool? ArtSale { get; set; }
    public bool? SizeSale { get; set; }
    public bool? ColorSale { get; set; }
    public bool? Pack1Sale { get; set; }
    public bool? Pack2Sale { get; set; }
    public bool? mRateSale { get; set; }

    public bool? BarcodeSale { get; set; }
    public bool? DiscPercentSale { get; set; }
    public bool? DiscountSale { get; set; }

    public string? TermAndConditionSale { get; set; }
    public string? WhatWeDoInSale { get; set; }
    public string? DescriptionSale { get; set; }

    // Purchase
    public bool? RemarksPurchase { get; set; }
    public bool? HSNPurchase { get; set; }
    public bool? ArtPurchase { get; set; }
    public bool? SizePurchase { get; set; }
    public bool? ColorPurchase { get; set; }
    public bool? Pack1Purchase { get; set; }
    public bool? Pack2Purchase { get; set; }
    public bool? mRatePurchase { get; set; }

    public bool? BarcodePurchase { get; set; }
    public bool? DiscPercentPurchase { get; set; }
    public bool? DiscountPurchase { get; set; }

    public string? TermAndConditionPurchase { get; set; }
    public string? WhatWeDoInPurchase { get; set; }
    public string? DescriptionPurchase { get; set; }
}
public class CommonSpResponseWithId
{
    public int Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int NewId { get; set; }
}
