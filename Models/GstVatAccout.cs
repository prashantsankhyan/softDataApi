using System;
using System.Collections.Generic;

namespace softDataApi.Models;

public partial class GstVatAccout
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public string? ClassName { get; set; }

    public decimal Rate { get; set; }

    public int? HeadingName { get; set; }

    public string? Type { get; set; }
}


public class GstVatAccountListDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string? ClassName { get; set; }
    public decimal Rate { get; set; }
    public int HeadingId { get; set; }
    public string? HeadingName { get; set; }
    public string? Type { get; set; }
    public string? EntrBy { get; set; }
}


