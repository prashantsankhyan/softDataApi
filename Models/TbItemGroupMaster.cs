using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace softDataApi.Models;

public partial class TbItemGroupMaster
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public string ItemGroupName { get; set; } = null!;

    public int? ItemGroup { get; set; }


}

public class ItemGroupMaster
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string? ItemGroupName { get; set; }
    public int? ItemGroup { get; set; } // Parent group

 
   
}

public class ItemGroupDTOComanyId
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string ItemGroupName { get; set; } = "";
    public int? ItemGroup { get; set; }
    public string? ParentGroupName { get; set; }
}

public class ItemGroupMasterById
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string? ItemGroupName { get; set; }
    public int? ItemGroup { get; set; } // Parent group
}
public class ResponseMessage
{
    public int ResponseCode { get; set; }
    public string? Message { get; set; }
}
