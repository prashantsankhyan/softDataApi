using System;
using System.Collections.Generic;

namespace softDataApi.Models;

public partial class TbStateMaster
{
    public int Id { get; set; }

    public string State { get; set; } = null!;

    public string? StateCode { get; set; }

    public virtual ICollection<TbCityMaster> TbCityMasters { get; set; } = new List<TbCityMaster>();
}

public partial class AddTbState
{
    public int Id { get; set; }

    public string? State { get; set; } = null!;

    public string? StateCode { get; set; }


}


