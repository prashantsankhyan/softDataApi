using System;
using System.Collections.Generic;

namespace softDataApi.Models;

public partial class TbHeadingNameMaster
{
    public int Id { get; set; }

    public string? HeadingName { get; set; }

    public int? CompanyId { get; set; }

    public string? EntrBy { get; set; }
}
public class CommonSpResponse
{
    public int Success { get; set; }
    public string? Message { get; set; }
}
