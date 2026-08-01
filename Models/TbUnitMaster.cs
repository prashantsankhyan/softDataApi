using System;
using System.Collections.Generic;

namespace softDataApi.Models;

public partial class TbUnitMaster
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public string? UnitName { get; set; }

    public string? Quantity { get; set; }

    public string? Decimal { get; set; }
}
