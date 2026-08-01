using System;
using System.Collections.Generic;

namespace softDataApi.Models;

public partial class TbGroupMaster
{
    public int? GroupId { get; set; }

    public int CompanyId { get; set; }

    public string? GroupName { get; set; } = null!;

    public int? AppearIn { get; set; }

    public int? GroupCategory { get; set; }

    public int? UnderGroup { get; set; }

    public string? AnnexureNo { get; set; }
}







public class GroupDto
{
    public int GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public int CompanyId { get; set; }

    public int? AppearIn { get; set; }
    public string? AppearInName { get; set; }

    public int? GroupCategory { get; set; }
    public string? GroupCategoryName { get; set; }

    public int? UnderGroup { get; set; }
    public string? UnderGroupName { get; set; }

    public string? AnnexureNo { get; set; }
}


public class GroupMasterZero
{
    public int GroupId { get; set; }
    public int CompanyId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public int? AppearIn { get; set; }
    public int? GroupCategory { get; set; }
    public int? UnderGroup { get; set; }
    public string? AnnexureNo { get; set; }
}

