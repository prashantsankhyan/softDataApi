using System;
using System.Collections.Generic;

namespace softDataApi.Models;

public partial class SubUserDetail
{
    public int Id { get; set; }

    public int? CompanyId { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Permissions { get; set; }

    public int ClientId { get; set; }
}

public class SubUserLoginRequest
{
    public string PhoneNumber { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int? CompanyId { get; set; }
}


public class SubUserLoginDto
{
    public int ResponseCode { get; set; }
    public string? Message { get; set; }

    public int? Id { get; set; }
    public int? CompanyId { get; set; }
    public string? CompanyName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Username { get; set; }
    public string? Permissions { get; set; }
    public int? ClientId { get; set; }
}


