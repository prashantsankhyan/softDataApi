using System;
using System.Collections.Generic;

namespace softDataApi.Models;

public partial class TbAgentMaster
{
    public int Id { get; set; }

    public string AgentName { get; set; } = null!;

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? PinCode { get; set; }

    public string? PhoneNumber { get; set; }

    public string? PanNo { get; set; }

    public string? Description { get; set; }

    public int CompanyId { get; set; }

    public string? EnterBy { get; set; }

    public DateTime? CreatedDate { get; set; }
}




public class AgentMasterDto
{
    public int Id { get; set; }          // 0 = Add, >0 = Edit
    public int CompanyId { get; set; }
    public string AgentName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PinCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? PanNo { get; set; }
    public string? Description { get; set; }
    public string? EnterBy { get; set; }
}

public class AddEditAgentResult
{
    public int? NewId { get; set; }
    public int? UpdatedId { get; set; }
    public int Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
