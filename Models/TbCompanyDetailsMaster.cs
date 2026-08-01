using System;
using System.Collections.Generic;

namespace softDataApi.Models;

public class CompanyDetailsDto
{
    public int CompanyId { get; set; }
    public string? CompanyName { get; set; }

    public int ClientId { get; set; }
    public string? ClientName { get; set; }

    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }

    public int? City { get; set; }
    public string? CityName { get; set; }

    public int? StateId { get; set; }
    public string? StateName { get; set; }

    public string? Zip { get; set; }
    public string? GSTNo { get; set; }
    public string? PANNo { get; set; }
    public string? SINNo { get; set; }

    public DateTime? InstallDate { get; set; }
    public DateTime? BoxStartingDate { get; set; }

    public int? FinisalYear { get; set; }        // ✅ FIXED (int)
    public bool? MaintainStock { get; set; }

    public DateTime? PermissionUpTo { get; set; } // ✅ FIXED (date)
}

public partial class TbCompanyDetailsMaster
{
    public int Id { get; set; }

    public string CompanyName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Email { get; set; }

    public string? Address { get; set; }

    public int? City { get; set; }

    public string? Zip { get; set; }

    public string? Gstno { get; set; }

    public string? Panno { get; set; }

    public string? Sinno { get; set; }

    public DateOnly? InstallDate { get; set; }

    public DateOnly? BoxStartingDate { get; set; }

    public int? FinisalYear { get; set; }

    public bool? MaintainStock { get; set; }

    public DateOnly? PermissionUpTo { get; set; }

    public int? ClientId { get; set; }
}

public class AddEditCompanyRequest
{
    public int Id { get; set; }
    public string? CompanyName { get; set; }
    public int ClientId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public int? City { get; set; }
    public string? Zip { get; set; }
    public string? GSTNo { get; set; }
    public string? PANNo { get; set; }
    public string? SINNo { get; set; }
    public DateTime? InstallDate { get; set; }
    public DateTime? BoxStartingDate { get; set; }
    public int? FinisalYear { get; set; }
    public bool? MaintainStock { get; set; }
    public DateTime? PermissionUpTo { get; set; }
}

public class CompanyIdResponse
{
    public int CompanyId { get; set; }
}