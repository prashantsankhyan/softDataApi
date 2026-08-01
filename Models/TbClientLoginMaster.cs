using System;
using System.Collections.Generic;

namespace softDataApi.Models;

public partial class TbClientLoginMaster
{
    public int Id { get; set; }

    public string ClientName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Email { get; set; }

    public int City { get; set; }

    public string? Zip { get; set; }

    public string? Address { get; set; }

    public string? Package { get; set; }

    public decimal? PackageAmount { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual TbCityMaster CityNavigation { get; set; } = null!;
}


public class AddEditClientRequest
{
    public int Id { get; set; }
    public string? ClientName { get; set; }
    public string? Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public int City { get; set; }
    public string? Zip { get; set; }
    public string? Address { get; set; }
    public string? Package { get; set; }
    public decimal? PackageAmount { get; set; }
    public bool IsActive { get; set; }
}
public class ClientDetailsDto
{
    public int Id { get; set; }
    public string? ClientName { get; set; }
    public string? Password { get; set; }   // ⚠ optional – see note below
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public int City { get; set; }
    public string? CityName { get; set; }
    public string? StateName { get; set; }
    public string? Zip { get; set; }
    public string? Address { get; set; }
    public string? Package { get; set; }
    public decimal? PackageAmount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}


public class ClientLoginResponse
{
    public int ResponseCode { get; set; }
    public int Id { get; set; }
    public string? ClientName { get; set; }
    public string? PhoneNumber { get; set; }

    public int CompanyId { get; set; }
    public string? CompanyName { get; set; }
}

public class ClientDetailsResponse
{
    public int ResponseCode { get; set; }

    public int Id { get; set; }
    public string? ClientName { get; set; }
    public string? Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public int City { get; set; }
    public string? Zip { get; set; }
    public string? Address { get; set; }
    public string? Package { get; set; }
    public decimal? PackageAmount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }

    public int CompanyId { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanyPhoneNumber { get; set; }

    public string? CompanyAddress { get; set; }


    


}

public class ClientLoginRequest
{
    public string PhoneNumber { get; set; } = null!;
    public string Password { get; set; } = null!;
}



