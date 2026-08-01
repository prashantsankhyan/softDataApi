using System;
using System.Collections.Generic;

namespace softDataApi.Models;

public partial class TbTransportMaster
{
    public int TransportId { get; set; }

    public int CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public string? GstNo { get; set; }
}
