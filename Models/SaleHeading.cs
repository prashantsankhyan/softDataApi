using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace softDataApi.Models;

public partial class SaleHeading
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

public class SaleHeadingDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string? TypeOfSale { get; set; }
    public string? Prefix { get; set; }
    public string? Suffix { get; set; }
    public string? TaxOnSaleType { get; set; }
    public string? NumberStartFrom { get; set; }

    public string? Permission { get; set; }
}
[Keyless]
public class SaleHeadingById
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string? TypeOfSale { get; set; }
    public string? Prefix { get; set; }
    public string? Suffix { get; set; }
    public string? TaxOnSaleType { get; set; }
    public string? NumberStartFrom { get; set; }

    public string? Permission { get; set; }
}


public class NextInvoiceNoDto
{
    public string? NextInvoiceNo { get; set; }
}
