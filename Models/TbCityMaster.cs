using System;
using System.Collections.Generic;

namespace softDataApi.Models;

public partial class TbCityMaster
{
    public int Id { get; set; }

    public string City { get; set; } = null!;

    public int? State { get; set; }

    public virtual TbStateMaster? StateNavigation { get; set; }

    public virtual ICollection<TbClientLoginMaster> TbClientLoginMasters { get; set; } = new List<TbClientLoginMaster>();
}

public class CityDetailsDto
{
    public int CityId { get; set; }
    public string? CityName { get; set; }
    public int StateId { get; set; }
    public string? StateName { get; set; }
    public string? StateCode { get; set; }
}
public class CityDetails
{
    public int Id { get; set; }
    public string? City { get; set; }
    public int State { get; set; }
}
