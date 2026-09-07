using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using softDataApi.Models;
using System.Data;
using System.Data.Common;

public static class DataReaderExt
{
    public static T? Get<T>(this DbDataReader r, string column)
    {
        var value = r[column];

        if (value == DBNull.Value || value == null)
            return default;

        // fix: handle empty strings
        if (value is string s && string.IsNullOrWhiteSpace(s))
            return default;

        return (T)Convert.ChangeType(value, typeof(T));
    }
}



namespace softDataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoftDataAPIController : ControllerBase
    {
        private readonly SoftDataContext context;

        public SoftDataAPIController(SoftDataContext context)
        {
            this.context = context;
        }
        [HttpGet("states")]
        public async Task<IActionResult> GetStates()
        {
            var states = await context.TbStateMasters.ToListAsync();
            return Ok(states);
        }

        [HttpPost("add-edit-state")]
        public async Task<IActionResult> AddEditState(AddTbState model)
        {
            await context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.addEditState @Id, @State, @StateCode",
                new SqlParameter("@Id", model.Id),
                new SqlParameter("@State", model.State),
                new SqlParameter("@StateCode", model.StateCode ?? (object)DBNull.Value)
            );

            return Ok(new { message = "Saved successfully" });
        }

        [HttpDelete("delete-state/{id}")]
        public async Task<IActionResult> DeleteState(int id)
        {
            try
            {
                var idParam = new SqlParameter("@Id", id);

                await context.Database.ExecuteSqlRawAsync(
                    "EXEC deleteState @Id",
                    idParam
                );

                return Ok(new
                {
                    success = true,
                    message = "State deleted successfully"
                });
            }
            catch (SqlException ex)
            {
                // This will catch RAISERROR from SQL
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error",
                    error = ex.Message
                });
            }
        }

        [HttpGet("get-state/{id}")]
        public async Task<IActionResult> GetStateById(int id)
        {
            try
            {
                var idParam = new SqlParameter("@Id", id);

                var states = await context.AddTbState
                    .FromSqlRaw("EXEC getStateById @Id", idParam)
                    .ToListAsync();

                return Ok(states);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error",
                    error = ex.Message
                });
            }
        }

        [HttpGet("get-city-details")]
        public async Task<IActionResult> GetCityDetails()
        {
            var data = await context.Set<CityDetailsDto>()
                .FromSqlRaw("EXEC dbo.getCityDetails")
                .AsNoTracking()
                .ToListAsync();

            return Ok(data);
        }


        [HttpPost("add-edit-city")]
        public async Task<IActionResult> AddEditCity(CityDetails model)
        {
            await context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.addEditCity @Id, @City, @State",
                new SqlParameter("@Id", model.Id),
                new SqlParameter("@City", model.City),
                new SqlParameter("@State", model.State)
            );

            return Ok(new { message = "City saved successfully" });
        }


        [HttpGet("get-city-by-id/{id}")]
        public async Task<IActionResult> GetCityById(int id)
        {
            var result = await context.CityDetails
                .FromSqlRaw(
                    "EXEC dbo.getCityById @Id",
                    new SqlParameter("@Id", id)
                )
                .AsNoTracking()
                .ToListAsync();   // ✅ materialize first

            var data = result.FirstOrDefault(); // ✅ client-side

            return Ok(data);
        }



        [HttpDelete("delete-city/{id}")]
        public async Task<IActionResult> DeleteCityById(int id)
        {
            try
            {
                await context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.deleteCityById @Id",
                    new SqlParameter("@Id", id)
                );

                return Ok(new { message = "City deleted successfully" });
            }
            catch (SqlException ex)
            {
                // This catches RAISERROR from SQL Server
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("add-edit-client")]
        public async Task<IActionResult> AddEditClient([FromBody] AddEditClientRequest model)
        {
            try
            {
                var result = await context.AddEditClientRequest
                    .FromSqlRaw(
                        @"EXEC dbo.addEditClient  
                    @Id, 
                    @ClientName, 
                    @Password, 
                    @PhoneNumber, 
                    @Email, 
                    @City, 
                    @Zip, 
                    @Address, 
                    @Package, 
                    @PackageAmount, 
                    @IsActive",
                        new SqlParameter("@Id", model.Id),
                        new SqlParameter("@ClientName", model.ClientName),
                        new SqlParameter("@Password", model.Password),
                        new SqlParameter("@PhoneNumber", model.PhoneNumber),
                        new SqlParameter("@Email", (object?)model.Email ?? DBNull.Value),
                        new SqlParameter("@City", model.City),
                        new SqlParameter("@Zip", (object?)model.Zip ?? DBNull.Value),
                        new SqlParameter("@Address", (object?)model.Address ?? DBNull.Value),
                        new SqlParameter("@Package", (object?)model.Package ?? DBNull.Value),
                        new SqlParameter("@PackageAmount", (object?)model.PackageAmount ?? DBNull.Value),
                        new SqlParameter("@IsActive", model.IsActive)
                    )
                    .AsNoTracking()
                    .ToListAsync();

                var data = result.FirstOrDefault();

                return Ok(new
                {
                    success = true,
                    message = model.Id > 0
                        ? "Client updated successfully"
                        : "Client added successfully",
                    data
                });
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                return Ok(new
                {
                    success = false,
                    message = "Phone number already exists",
                    data = (object?)null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message,
                    data = (object?)null
                });
            }
        }



        [HttpGet("get-all-clients")]
        public async Task<IActionResult> GetAllClients()
        {
            var data = await context.Set<ClientDetailsDto>()
                .FromSqlRaw("EXEC dbo.getAllClientDetais")
                .AsNoTracking()
                .ToListAsync();   // ✅ materialize first

            return Ok(new
            {
                success = true,
                count = data.Count,
                data
            });
        }


        [HttpGet("get-client/{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            var result = await context.AddEditClientRequest
                .FromSqlRaw(
                    "EXEC dbo.getClientById @Id",
                    new SqlParameter("@Id", id)
                )
                .AsNoTracking()
                .ToListAsync();   // ✅ materialize first

            if (id == 0)
                return Ok(result);          // return all clients

            return Ok(result.FirstOrDefault()); // return single client
        }


        [HttpDelete("delete-client/{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            try
            {
                await context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.deleteClinetMaster @Id",
                    new SqlParameter("@Id", id)
                );

                return Ok(new
                {
                    success = true,
                    message = "Client deleted successfully"
                });
            }
            catch (SqlException ex)
            {
                // Captures RAISERROR from SQL Server
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error",
                    error = ex.Message
                });
            }
        }

        [HttpPost("add-edit-company")]
        public async Task<IActionResult> AddEditCompany([FromBody] AddEditCompanyRequest model)
        {
            var result = await context.TbCompanyDetailsMasters
                .FromSqlRaw(
                    @"EXEC dbo.addEditCompanyDetails
              @Id,
              @CompanyName,
              @ClientId,
              @PhoneNumber,
              @Email,
              @Address,
              @City,
              @Zip,
              @GSTNo,
              @PANNo,
              @SINNo,
              @InstallDate,
              @BoxStartingDate,
              @FinisalYear,
              @MaintainStock,
              @PermissionUpTo",
                    new SqlParameter("@Id", model.Id),
                    new SqlParameter("@CompanyName", model.CompanyName),
                    new SqlParameter("@ClientId", model.ClientId),
                    new SqlParameter("@PhoneNumber", model.PhoneNumber),
                    new SqlParameter("@Email", (object?)model.Email ?? DBNull.Value),
                    new SqlParameter("@Address", (object?)model.Address ?? DBNull.Value),
                    new SqlParameter("@City", (object?)model.City ?? DBNull.Value),
                    new SqlParameter("@Zip", (object?)model.Zip ?? DBNull.Value),
                    new SqlParameter("@GSTNo", (object?)model.GSTNo ?? DBNull.Value),
                    new SqlParameter("@PANNo", (object?)model.PANNo ?? DBNull.Value),
                    new SqlParameter("@SINNo", (object?)model.SINNo ?? DBNull.Value),
                    new SqlParameter("@InstallDate", (object?)model.InstallDate ?? DBNull.Value),
                    new SqlParameter("@BoxStartingDate", (object?)model.BoxStartingDate ?? DBNull.Value),
                    new SqlParameter("@FinisalYear", (object?)model.FinisalYear ?? DBNull.Value),
                    new SqlParameter("@MaintainStock", (object?)model.MaintainStock ?? DBNull.Value),
                    new SqlParameter("@PermissionUpTo", (object?)model.PermissionUpTo ?? DBNull.Value)
                )
                .AsNoTracking()
                .ToListAsync();   // ✅ materialize first (IMPORTANT)

            var data = result.FirstOrDefault();

            return Ok(new
            {
                success = true,
                message = model.Id > 0
                    ? "Company updated successfully"
                    : "Company added successfully",
                data
            });
        }


        [HttpGet("subuser/company/{phoneNumber}")]
        public async Task<IActionResult> GetCompanyByPhoneNumber(string phoneNumber)
        {
            try
            {
                var companies = new List<object>();

                using (var conn = context.Database.GetDbConnection())
                {
                    await conn.OpenAsync();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "dbo.GetCompanyIdByPhoneNumber";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        var param = cmd.CreateParameter();
                        param.ParameterName = "@PhoneNumber";
                        param.Value = phoneNumber?.Trim() ?? "";
                        cmd.Parameters.Add(param);

                        using var reader = await cmd.ExecuteReaderAsync();

                        while (await reader.ReadAsync())
                        {
                            companies.Add(new
                            {
                                companyId = reader.GetInt32(
                                    reader.GetOrdinal("companyId")
                                ),

                                companyName = reader.GetString(
                                    reader.GetOrdinal("companyName")
                                ),

                                phoneNumber = reader.IsDBNull(
                                    reader.GetOrdinal("phoneNumber")
                                )
                                    ? ""
                                    : reader.GetString(
                                        reader.GetOrdinal("phoneNumber")
                                    )
                            });
                        }
                    }
                }

                if (companies.Count == 0)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "No company found for this phone number.",
                        data = new List<object>()
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Company found.",
                    data = companies
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        [HttpGet("get-all-companies")]
        public async Task<IActionResult> GetAllCompanyDetails()
        {
            var data = await context.Set<CompanyDetailsDto>()
                .FromSqlRaw("EXEC dbo.getAllCompanyDetails")
                .AsNoTracking()
                .ToListAsync();

            return Ok(new
            {
                success = true,
                count = data.Count,
                data
            });
        }

        [HttpGet("get-company-by-client-id/{clientId}")]
        public async Task<IActionResult> GetCompanyByClientId(int clientId)
        {
            try
            {
                var result = await context.TbCompanyDetailsMasters
                    .FromSqlRaw(
                        "EXEC dbo.getCompanyDetailsByClientId @ClientId",
                        new SqlParameter("@ClientId", clientId)
                    )
                    .AsNoTracking()
                    .ToListAsync();

                if (result == null || result.Count == 0)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "No company found for this ClientId.",
                        data = new List<object>()
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Company details found successfully.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error while getting company details.",
                    error = ex.Message,
                    data = new List<object>()
                });
            }
        }


        [HttpGet("get-company-by-id/{id}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            var result = await context.TbCompanyDetailsMasters
                .FromSqlRaw(
                    "EXEC dbo.getCompanyDetailById @Id",
                    new SqlParameter("@Id", id)
                )
                .AsNoTracking()
                .ToListAsync();        // ✅ materialize first

            var data = result.FirstOrDefault(); // ✅ client-side

            return Ok(data);
        }

        [HttpDelete("delete-company/{id}")]
        public async Task<IActionResult> DeleteCompanyById(int id)
        {
            try
            {
                await context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.deleteCompanyDetailById @Id",
                    new SqlParameter("@Id", id)
                );

                return Ok(new { message = "Company deleted successfully" });
            }
            catch (SqlException ex)
            {
                // Catches RAISERROR thrown from SQL Server
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        //[HttpPost("client-login")]
        //public async Task<IActionResult> ClientLogin([FromBody] ClientLoginRequest model)
        //{
        //    var result = await context.ClientDetailsResponse
        //        .FromSqlRaw(
        //            @"EXEC dbo.clientLoginMaster 
        //      @PhoneNumber, 
        //      @Password",
        //            new SqlParameter("@PhoneNumber", model.PhoneNumber),
        //            new SqlParameter("@Password", model.Password)
        //        )
        //        .AsNoTracking()
        //        .ToListAsync();   // ✅ must materialize

        //    var data = result.FirstOrDefault();

        //    return Ok(new
        //    {
        //        success = data?.ResponseCode == 1,
        //        responseCode = data?.ResponseCode ?? 0,
        //        message = data?.ResponseCode switch
        //        {
        //            1 => "Login successful",
        //            2 => "Account inactive or package expired",
        //            _ => "Invalid phone number or password"
        //        },
        //        data
        //    });
        //}


        [HttpPost("client-login")]
        public async Task<IActionResult> ClientLogin([FromBody] ClientLoginRequest model)
        {
            try
            {
                var result = await context.ClientDetailsResponse
                    .FromSqlRaw(
                        @"EXEC dbo.clientLoginMaster 
                    @PhoneNumber, 
                    @Password",
                        new SqlParameter(
                            "@PhoneNumber",
                            (object?)model.PhoneNumber?.Trim() ?? DBNull.Value
                        ),
                        new SqlParameter(
                            "@Password",
                            (object?)model.Password?.Trim() ?? DBNull.Value
                        )
                    )
                    .AsNoTracking()
                    .ToListAsync();

                if (result == null || !result.Any())
                {
                    return Ok(new
                    {
                        success = false,
                        responseCode = 0,
                        message = "Invalid phone number or password",
                        data = new List<object>()
                    });
                }

                int responseCode = result.First().ResponseCode;

                return Ok(new
                {
                    success = responseCode == 1,
                    responseCode = responseCode,
                    message = responseCode switch
                    {
                        1 => "Login successful",
                        2 => "Account inactive or package expired",
                        _ => "Invalid phone number or password"
                    },
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    responseCode = 500,
                    message = ex.Message,
                    data = new List<object>()
                });
            }
        }

        [HttpGet("get-company-detail/{companyId}")]
        public async Task<IActionResult> GetCompanyDetailById(int companyId)
        {
            var result = await context.TbCompanyDetailsMasters
                .FromSqlRaw(
                    "EXEC dbo.getCompanyDetailById @Id",
                    new SqlParameter("@Id", companyId)
                )
                .AsNoTracking()
                .ToListAsync();

            // when id = 0 → returns all companies
            return Ok(new
            {
                success = true,
                count = result.Count,
                data = result
            });
        }

        [HttpPost("create-sub-user")]
        public async Task<IActionResult> CreateSubUser([FromBody] SubUserDetail model)
        {
            var successParam = new SqlParameter("@Success", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 255)
            {
                Direction = ParameterDirection.Output
            };

            await context.Database.ExecuteSqlRawAsync(
                @"EXEC dbo.createSubUser
            @CompanyId,
            @PhoneNumber,
            @Username,
            @Password,
            @Permissions,
            @ClientId,
            @Success OUTPUT,
            @Message OUTPUT",
                new SqlParameter("@CompanyId", model.CompanyId),
                new SqlParameter("@PhoneNumber", model.PhoneNumber),
                new SqlParameter("@Username", model.Username),
                new SqlParameter("@Password", model.Password),
                new SqlParameter("@Permissions", model.Permissions),
                new SqlParameter("@ClientId", model.ClientId),
                successParam,
                messageParam
            );

            return Ok(new
            {
                success = successParam.Value != DBNull.Value && (bool)successParam.Value,
                message = messageParam.Value?.ToString()
            });
        }


        [HttpGet("get-sub-user-details/{companyId}")]
        public async Task<IActionResult> GetSubUserDetailsByCompanyId(int companyId)
        {
            var result = await context.SubUserDetails
                .FromSqlRaw(
                    "EXEC dbo.getSubUserDetailsByCompanyId @CompanyId",
                    new SqlParameter("@CompanyId", companyId)
                )
                .AsNoTracking()
                .ToListAsync();

            return Ok(new
            {
                success = true,
                count = result.Count,
                data = result
            });
        }

        [HttpDelete("delete-sub-user/{id}")]
        public async Task<IActionResult> DeleteSubUserById(int id)
        {
            try
            {
                await context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.DeleteSubUserById @Id",
                    new SqlParameter("@Id", id)
                );

                return Ok(new { message = "Sub user deleted successfully" });
            }
            catch (SqlException ex)
            {
                // Catches RAISERROR thrown from SQL Server
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("sub-user-login")]
        public async Task<IActionResult> SubUserLogin([FromBody] SubUserLoginRequest model)
        {
            var result = await context.SubUserLoginDtos
                .FromSqlRaw(
                    @"EXEC dbo.subUserLogin
                @PhoneNumber,
                @Username,
                @Password,
                @CompanyId",
                    new SqlParameter("@PhoneNumber", model.PhoneNumber),
                    new SqlParameter("@Username", model.Username),
                    new SqlParameter("@Password", model.Password),
                    new SqlParameter("@CompanyId", (object?)model.CompanyId ?? DBNull.Value)
                )
                .AsNoTracking()
                .ToListAsync();   // ✅ materialize first

            var data = result.FirstOrDefault();

            return Ok(new
            {
                success = data?.ResponseCode == 1,
                responseCode = data?.ResponseCode ?? 0,
                message = data?.Message,
                data = data?.ResponseCode == 1 ? data : null
            });
        }


        [HttpPost("save-group")]
        public async Task<IActionResult> InsertOrUpdateGroup([FromBody] TbGroupMaster model)
        {
            try
            {
                var resultParam = new SqlParameter
                {
                    ParameterName = "@ReturnVal",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                await context.Database.ExecuteSqlRawAsync(
                    @"EXEC @ReturnVal = dbo.sp_InsertGroup
                @GroupId,
                @CompanyId,
                @GroupName,
                @AppearIn,
                @GroupCategory,
                @UnderGroup,
                @AnnexureNo",
                    resultParam,
                    new SqlParameter("@GroupId", model.GroupId ?? 0),
                    new SqlParameter("@CompanyId", model.CompanyId),
                    new SqlParameter("@GroupName", model.GroupName),
                    new SqlParameter("@AppearIn", (object?)model.AppearIn ?? DBNull.Value),
                    new SqlParameter("@GroupCategory", (object?)model.GroupCategory ?? DBNull.Value),
                    new SqlParameter("@UnderGroup", (object?)model.UnderGroup ?? DBNull.Value),
                    new SqlParameter("@AnnexureNo", (object?)model.AnnexureNo ?? DBNull.Value)
                );

                int result = (int)resultParam.Value;

                return result switch
                {
                    1 => Ok(new { success = true, message = "Group inserted successfully" }),
                    2 => Conflict(new { success = false, message = "Group already exists" }),
                    3 => Ok(new { success = true, message = "Group updated successfully" }),
                    4 => NotFound(new { success = false, message = "Group not found" }),
                    _ => BadRequest(new { success = false, message = "Invalid input or error occurred" })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Server error",
                    error = ex.Message
                });
            }
        }

        [HttpGet("check-group-exists/{companyId}/{groupName}/{groupId}")]
        public IActionResult CheckGroupExists(
     int companyId,
     string groupName,
     int groupId = 0)
        {
            try
            {
                groupName = groupName?.Trim() ?? string.Empty;

                bool exists = context.TbGroupMasters.Any(x =>
                    x.GroupId != groupId &&
                    x.GroupName != null &&

                    // Match name
                    x.GroupName.Trim().ToLower() ==
                    groupName.ToLower()

                    &&

                    // Company logic
                    (
                        x.CompanyId == 0 ||           // global
                        x.CompanyId == companyId      // same company
                    )
                );

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        existsData = exists,
                        message = exists
                            ? "Group already exists"
                            : "Group available"
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet("group-categories")]
        public async Task<IActionResult> GetAllGroupCategories()
        {
            try
            {
                var data = await context.TbGroupCategoryMasters
                    .AsNoTracking()
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    count = data.Count,
                    data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch group categories",
                    error = ex.Message
                });
            }
        }

        [HttpGet("group-Master/{companyId}")]
        public async Task<IActionResult> GetGroupsByCompanyId(int companyId)
        {
            try
            {
                var data = await context.Groups
                    .FromSqlRaw(
                        "EXEC dbo.GetGroupsByCompanyId @CompanyId",
                        new SqlParameter("@CompanyId", companyId)
                    )
                    .AsNoTracking()
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    count = data.Count,
                    data
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch groups",
                    error = ex.Message
                });
            }
        }


        [HttpGet("groups/company-zero")]
        public async Task<IActionResult> GetGroupMasterCompanyZero()
        {
            try
            {
                var data = await context.GroupMasterZero
                    .FromSqlRaw("EXEC dbo.GetGroupMasterCompanyZero")
                    .AsNoTracking()
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    count = data.Count,
                    data
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch group master data",
                    error = ex.Message
                });
            }
        }


        [HttpGet("group-Maste-ById/{id}")]
        public async Task<IActionResult> GetGroupById(int id)
        {
            try
            {
                var data = context.GroupMasterZero
                    .FromSqlRaw(
                        "EXEC dbo.GetGroupById @GroupId",
                        new SqlParameter("@GroupId", id)
                    )
                    .AsEnumerable()          // ✅ switch to client-side
                    .FirstOrDefault();       // ✅ now safe

                if (data == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Group not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch group details",
                    error = ex.Message
                });
            }
        }


        [HttpGet("item-group-master/{companyId}")]
        public async Task<IActionResult> GetItemGroupsByCompany(int companyId)
        {
            try
            {
                var companyParam = new SqlParameter("@companyId", companyId);

                var data = await context.ItemGroupDTOComanyId
                    .FromSqlRaw("EXEC dbo.getItemGroupsByCompany @companyId", companyParam)
                    .AsNoTracking()
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    count = data.Count,
                    data = data
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "Failed to fetch item groups.",
                    error = ex.Message
                });
            }
        }

        [HttpPost("add-edit-item-group")]
        public async Task<IActionResult> AddEditItemGroup([FromBody] ItemGroupMaster model)
        {
            try
            {
                ResponseMessage? response = null;

                var query = context.Database
                    .SqlQueryRaw<ResponseMessage>(
                        @"EXEC dbo.addEditItemGroupMaster 
                  @Id,
                  @companyId,
                  @itemGroupName,
                  @itemGroup",
                        new SqlParameter("@Id", model.Id),
                        new SqlParameter("@companyId", model.CompanyId),
                        new SqlParameter("@itemGroupName", model.ItemGroupName),
                        new SqlParameter("@itemGroup", (object?)model.ItemGroup ?? DBNull.Value)
                    )
                    .AsAsyncEnumerable();

                // ✅ CORRECT WAY to get first row from IAsyncEnumerable
                await foreach (var item in query)
                {
                    response = item;
                    break;
                }

                return Ok(new
                {
                    success = response?.ResponseCode == 1,
                    message = response?.Message
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to save item group",
                    error = ex.Message
                });
            }
        }

        [HttpGet("item-group-masterById/{id}")]
        public async Task<IActionResult> GetItemGroupMasterById(int id)
        {
            try
            {
                var data = await context.ItemGroupMasterById
                    .FromSqlRaw(
                        "EXEC dbo.getTbItemGroupMasterById @Id",
                        new SqlParameter("@Id", id)
                    )
                    .AsNoTracking()
                    .ToListAsync();

                if (data == null || data.Count == 0)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Item group not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = data.First()
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch item group",
                    error = ex.Message
                });
            }
        }


        [HttpDelete("item-group-master-delete/{id}")]
        public async Task<IActionResult> DeleteItemGroupMasterById(int id)
        {
            try
            {
                var idParam = new SqlParameter("@Id", id);

                var resultParam = new SqlParameter("@Result", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                await context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.deleteTbItemGroupMasterById @Id, @Result OUTPUT",
                    idParam,
                    resultParam
                );

                int result = (int)resultParam.Value;

                return result switch
                {
                    1 => Ok(new
                    {
                        success = true,
                        message = "Item group deleted successfully"
                    }),

                    -1 => Ok(new   // ✅ keep 200
                    {
                        success = false,
                        message = "Item group not found"
                    }),

                    -2 => Ok(new   // ✅ THIS IS THE KEY FIX
                    {
                        success = false,
                        message = "Cannot delete item group. Items exist under this group."
                    }),

                    _ => StatusCode(500, new
                    {
                        success = false,
                        message = "Unknown error occurred"
                    })
                };
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new   // ❗ SQL error = real server issue
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to delete item group",
                    error = ex.Message
                });
            }
        }


        [HttpGet("unit-master/company/{companyId}")]
        public async Task<IActionResult> GetUnitMasterByCompanyId(int companyId)
        {
            try
            {
                var companyParam = new SqlParameter("@companyId", companyId);

                var data = await context.TbUnitMasters
                    .FromSqlRaw(
                        "EXEC dbo.getTbUnitMasterByCompanyId @companyId",
                        companyParam
                    )
                    .AsNoTracking()
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    count = data.Count,
                    data
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch unit master data",
                    error = ex.Message
                });
            }
        }


        [HttpPost("unit-master/add-edit")]
        public async Task<IActionResult> AddEditUnitMaster([FromBody] TbUnitMaster model)
        {
            try
            {
                using var connection = context.Database.GetDbConnection();

                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "dbo.addEditTbUnitMaster";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Id", model.Id));
                command.Parameters.Add(new SqlParameter("@companyId", model.CompanyId));
                command.Parameters.Add(new SqlParameter("@unitName", model.UnitName ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@quantity", model.Quantity ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@decimal", model.Decimal ?? (object)DBNull.Value));

                using var reader = await command.ExecuteReaderAsync();

                int status = 0;
                string message = "";

                if (await reader.ReadAsync())
                {
                    status = Convert.ToInt32(reader["Status"]);
                    message = Convert.ToString(reader["Message"]) ?? "";
                }

                if (status == 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = message
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = message
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to save unit master",
                    error = ex.Message
                });
            }
        }

        [HttpGet("unit-master/{id}")]
        public async Task<IActionResult> GetUnitMasterById(int id)
        {
            try
            {
                var data = context.TbUnitMasters
                    .FromSqlRaw(
                        "EXEC dbo.getTbUnitMasterById @Id",
                        new SqlParameter("@Id", id)
                    )
                    .AsNoTracking()
                    .AsEnumerable()          // ✅ VERY IMPORTANT
                    .FirstOrDefault();       // ✅ safe now

                if (data == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Unit not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch unit",
                    error = ex.Message
                });
            }
        }


        [HttpDelete("unit-master-delete/{id}")]
        public async Task<IActionResult> DeleteUnitMasterById(int id)
        {
            try
            {
                var resultParam = new SqlParameter("@Result", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                await context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.deleteTbUnitMasterById @Id, @Result OUTPUT",
                    new SqlParameter("@Id", id),
                    resultParam
                );

                int result = (int)resultParam.Value;

                return result switch
                {
                    1 => Ok(new
                    {
                        success = true,
                        message = "Unit deleted successfully"
                    }),

                    -1 => Ok(new   // ✅ 200
                    {
                        success = false,
                        message = "Unit not found"
                    }),

                    -2 => Ok(new   // ✅ 200
                    {
                        success = false,
                        message = "Unit is in use and cannot be deleted"
                    }),

                    _ => Ok(new
                    {
                        success = false,
                        message = "Unexpected error occurred"
                    })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to delete unit",
                    error = ex.Message
                });
            }
        }


        [HttpPost("agent-master-add-edit")]
        public async Task<IActionResult> AddEditAgentMaster([FromBody] AgentMasterDto model)
        {
            try
            {
                await using var conn = context.Database.GetDbConnection();
                await conn.OpenAsync();

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = "addEditAgentMaster";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Id", model.Id));
                cmd.Parameters.Add(new SqlParameter("@CompanyId", model.CompanyId));
                cmd.Parameters.Add(new SqlParameter("@AgentName", model.AgentName));
                cmd.Parameters.Add(new SqlParameter("@Address", (object?)model.Address ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@City", (object?)model.City ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@PinCode", (object?)model.PinCode ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@PhoneNumber", (object?)model.PhoneNumber ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@PanNo", (object?)model.PanNo ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Description", (object?)model.Description ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@EnterBy", (object?)model.EnterBy ?? DBNull.Value));

                await using var reader = await cmd.ExecuteReaderAsync();

                if (!reader.Read())
                {
                    return Ok(new
                    {
                        success = false,
                        message = "No response from database"
                    });
                }

                // Read dynamically
                var success = reader.GetInt32(reader.GetOrdinal("Success"));
                var message = reader.GetString(reader.GetOrdinal("Message"));

                int? id = null;

                if (HasColumn(reader, "NewId"))
                    id = Convert.ToInt32(reader["NewId"]);

                if (HasColumn(reader, "UpdatedId"))
                    id = Convert.ToInt32(reader["UpdatedId"]);

                return Ok(new
                {
                    success = success == 1,
                    message,
                    id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to save agent",
                    error = ex.Message
                });
            }
        }
        private bool HasColumn(DbDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        [HttpGet("agent-master/{companyId}")]
        public async Task<IActionResult> GetAgentMasterByCompanyId(int companyId)
        {
            try
            {
                var data = await context.TbAgentMasters
                    .FromSqlRaw(
                        "EXEC dharmesh.getAgentMasterByCompanyId @CompanyId",
                        new SqlParameter("@CompanyId", companyId)
                    )
                    .AsNoTracking()
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    count = data.Count,
                    data
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch agents",
                    error = ex.Message
                });
            }
        }

        [HttpGet("get-agent-by-id/{id}")]
        public async Task<IActionResult> GetAgentById(int id)
        {
            try
            {
                var param = new SqlParameter("@Id", id);

                var agent = (await context.TbAgentMasters
                    .FromSqlRaw("EXEC getAgentMasterById @Id", param)
                    .ToListAsync())     // ✅ MATERIALIZE FIRST
                    .FirstOrDefault();  // ✅ CLIENT SIDE

                if (agent == null)
                {
                    return NotFound(new { success = false, message = "Agent not found" });
                }

                return Ok(new { success = true, data = agent });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch agent",
                    error = ex.Message
                });
            }
        }




        [HttpDelete("agent-master-delete/{id}")]
        public async Task<IActionResult> DeleteAgentMasterById(int id)
        {
            try
            {
                var idParam = new SqlParameter("@Id", id);

                int rowsAffected = await context.Database.ExecuteSqlRawAsync(
                    "EXEC dharmesh.deleteAgentMasterById @Id",
                    idParam
                );

                // ✅ Only success when actual delete happened
                if (rowsAffected > 0)
                {
                    return Ok(new
                    {
                        success = true,
                        message = "Agent deleted successfully"
                    });
                }

                // ❌ Not deleted (either not found or in use)
                return Ok(new
                {
                    success = false,
                    message = "Agent cannot be deleted (either not found or already used in accounts)"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to delete agent",
                    error = ex.Message
                });
            }
        }


        [HttpPost("category-master-add-edit")]
        public async Task<IActionResult> AddEditCategory(TbCategoryMaster model)
        {
            try
            {
                var parameters = new[]
                {
            new SqlParameter("@categoryId", model.CategoryId),
            new SqlParameter("@companyId", model.CompanyId),
            new SqlParameter("@categoryName", model.CategoryName),
            new SqlParameter("@categoryType", model.CategoryType),
            new SqlParameter("@result", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            }
        };

                await context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.addEditTbCategoryMaster @categoryId, @companyId, @categoryName, @categoryType, @result OUTPUT",
                    parameters
                );

                int result = (int)parameters[4].Value;

                return result switch
                {
                    1 => Ok(new { success = true, message = "Category added successfully" }),
                    2 => Ok(new { success = true, message = "Category updated successfully" }),
                    -1 => BadRequest(new { success = false, message = "Duplicate category name" }),
                    -2 => NotFound(new { success = false, message = "Category not found" }),
                    _ => StatusCode(500, new { success = false, message = "Unknown error" })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to save category",
                    error = ex.Message
                });
            }
        }

        [HttpGet("category-master-by-company/{companyId}")]
        public async Task<IActionResult> GetCategoryMasterByCompanyId(int companyId)
        {
            try
            {
                var companyParam = new SqlParameter("@companyId", companyId);

                var data = await context.TbCategoryMasters
                    .FromSqlRaw(
                        "EXEC dbo.getTbCategoryMasterByCompanyId @companyId",
                        companyParam
                    )
                    .AsNoTracking()
                    .ToListAsync();   // ✅ IMPORTANT

                return Ok(new
                {
                    success = true,
                    count = data.Count,
                    data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch categories",
                    error = ex.Message
                });
            }
        }


        [HttpGet("category-master-by-id/{categoryId}")]
        public async Task<IActionResult> GetCategoryMasterById(int categoryId)
        {
            try
            {
                var param = new SqlParameter("@categoryId", categoryId);

                var data = await context.TbCategoryMasters
                    .FromSqlRaw(
                        "EXEC dbo.getTbCategoryMasterById @categoryId",
                        param
                    )
                    .AsNoTracking()
                    .ToListAsync();   // ✅ IMPORTANT

                return Ok(new
                {
                    success = true,
                    data = data.FirstOrDefault()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch category",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("category-master-delete/{categoryId}")]
        public async Task<IActionResult> DeleteCategoryMasterById(int categoryId)
        {
            try
            {
                var resultParam = new SqlParameter("@Result", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                await context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.deleteTbCategoryMasterById @categoryId, @Result OUTPUT",
                    new SqlParameter("@categoryId", categoryId),
                    resultParam
                );

                int result = (int)(resultParam.Value ?? 0);

                return result switch
                {
                    1 => Ok(new
                    {
                        success = true,
                        message = "Category deleted successfully"
                    }),

                    -1 => Ok(new
                    {
                        success = false,
                        message = "Category not found"
                    }),

                    -2 => Ok(new   // ✅ Category in use
                    {
                        success = false,
                        message = "Category is used in Item Master and cannot be deleted"
                    }),

                    _ => StatusCode(500, new
                    {
                        success = false,
                        message = "Unexpected error occurred"
                    })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to delete category",
                    error = ex.Message
                });
            }
        }


        [HttpGet("transport-master")]
        public async Task<IActionResult> GetTransportMaster(
    [FromQuery] int? companyId,
    [FromQuery] int? transportId)
        {
            try
            {
                var data = await context.TbTransportMasters
                    .FromSqlRaw(
                        "EXEC dbo.sp_Get_TransportMaster @companyId, @transportId",
                        new SqlParameter("@companyId", (object?)companyId ?? DBNull.Value),
                        new SqlParameter("@transportId", (object?)transportId ?? DBNull.Value)
                    )
                    .AsNoTracking()
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    count = data.Count,
                    data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch transport master",
                    error = ex.Message
                });
            }
        }


        [HttpPost("transport-master-add-edit")]
        public async Task<IActionResult> AddEditTransportMaster([FromBody] TbTransportMaster model)
        {
            try
            {
                var parameters = new[]
                {
            new SqlParameter("@transportId", model.TransportId),
            new SqlParameter("@companyId", model.CompanyId),
            new SqlParameter("@name", model.Name),
            new SqlParameter("@phone", (object?)model.Phone ?? DBNull.Value),
            new SqlParameter("@gstNo", (object?)model.GstNo ?? DBNull.Value)
        };

                await context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.sp_AddEdit_TransportMaster @transportId, @companyId, @name, @phone, @gstNo",
                    parameters
                );

                return Ok(new
                {
                    success = true,
                    message = model.TransportId == 0
                        ? "Transport added successfully"
                        : "Transport updated successfully"
                });
            }
            catch (SqlException ex) when (ex.Number == 50001)
            {
                // ✅ Duplicate transport name
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to save transport",
                    error = ex.Message
                });
            }
        }


        [HttpDelete("transport-master-delete/{id}")]
        public async Task<IActionResult> DeleteTransportMaster(int id)
        {
            try
            {
                var returnParam = new SqlParameter
                {
                    ParameterName = "@ReturnVal",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                await context.Database.ExecuteSqlRawAsync(
                    @"DECLARE @Result INT;
              EXEC @Result = dbo.sp_Delete_TransportMaster @transportId;
              SET @ReturnVal = @Result;",
                    new SqlParameter("@transportId", id),
                    returnParam
                );

                int result = (int)(returnParam.Value ?? 0);

                return result switch
                {
                    1 => Ok(new
                    {
                        success = true,
                        message = "Transport deleted successfully"
                    }),

                    0 => Ok(new
                    {
                        success = false,
                        message = "Transport not found"
                    }),

                    -1 => Ok(new
                    {
                        success = false,
                        message = "Transport is used in Account Master and cannot be deleted"
                    }),

                    _ => StatusCode(500, new
                    {
                        success = false,
                        message = "Unexpected result from database"
                    })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to delete transport",
                    error = ex.Message
                });
            }
        }


        [HttpPost("heading-name-add-edit")]
        public async Task<IActionResult> AddEditHeadingName([FromBody] TbHeadingNameMaster model)
        {
            try
            {
                var idParam = new SqlParameter("@id", model.Id);
                var headingNameParam = new SqlParameter("@headingName", model.HeadingName);
                var companyIdParam = new SqlParameter("@companyId", model.CompanyId);
                var entrByParam = new SqlParameter("@entrBy", model.EntrBy);

                var result = context.CommonSpResponse
     .FromSqlRaw(
         "EXEC dbo.addEditHeadingName @id, @headingName, @companyId, @entrBy",
         idParam, headingNameParam, companyIdParam, entrByParam
     )
     .AsNoTracking()
     .AsEnumerable()        // ✅ break EF composition
     .FirstOrDefault();     // ✅ client-side

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = 0,
                    message = ex.Message
                });
            }
        }

        [HttpGet("heading-names-by-company/{companyId}")]
        public IActionResult GetHeadingNamesByCompanyId(int companyId)
        {
            try
            {
                var companyIdParam = new SqlParameter("@companyId", companyId);

                var result = context.TbHeadingNameMasters
                    .FromSqlRaw(
                        "EXEC dbo.getHeadingNamesByCompanyId @companyId",
                        companyIdParam
                    )
                    .AsNoTracking()
                    .AsEnumerable()   // ✅ break EF composition (safe with EXEC)
                    .ToList();

                return Ok(new
                {
                    success = 1,
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = 0,
                    message = ex.Message
                });
            }
        }

        [HttpGet("heading-name-by-id/{id}")]
        public IActionResult GetHeadingNameById(int id)
        {
            try
            {
                var idParam = new SqlParameter("@id", id);

                var result = context.TbHeadingNameMasters
                    .FromSqlRaw(
                        "EXEC dbo.getHeadingNameById @id",
                        idParam
                    )
                    .AsNoTracking()
                    .AsEnumerable()   // ✅ avoid EF Core non-composable error
                    .FirstOrDefault();

                if (result == null)
                {
                    return Ok(new
                    {
                        success = 0,
                        message = "Heading name not found"
                    });
                }

                return Ok(new
                {
                    success = 1,
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = 0,
                    message = ex.Message
                });
            }
        }

        [HttpDelete("heading-name-delete/{id}")]
        public async Task<IActionResult> DeleteHeadingNameById(int id)
        {
            try
            {
                var resultParam = new SqlParameter("@Result", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                await context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.deleteHeadingNameById @id, @Result OUTPUT",
                    new SqlParameter("@id", id),
                    resultParam
                );

                int result = (int)resultParam.Value;

                if (result == -1)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "Heading name is already used and cannot be deleted"
                    });
                }

                if (result == 0)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "Heading name not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Heading name deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to delete heading name",
                    error = ex.Message
                });
            }
        }



        [HttpPost("gst-vat-account-add-edit")]
        public IActionResult AddEditGstVatAccount([FromBody] GstVatAccout model)
        {
            try
            {
                var result = context.CommonSpResponse
                    .FromSqlRaw(
                        "EXEC dbo.addEditGstVatAccount @id, @companyId, @className, @rate, @headingName, @type",
                        new SqlParameter("@id", model.Id),
                        new SqlParameter("@companyId", model.CompanyId),
                        new SqlParameter("@className", model.ClassName),
                        new SqlParameter("@rate", model.Rate),
                        new SqlParameter("@headingName", model.HeadingName),
                        new SqlParameter("@type", model.Type)
                    )
                    .AsNoTracking()
                    .AsEnumerable()     // ✅ important for EXEC
                    .FirstOrDefault();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = 0,
                    message = ex.Message
                });
            }
        }
        [HttpGet("gst-vat-account-by-company/{companyId}")]
        public IActionResult GetGstVatAccountByCompanyId(int companyId)
        {
            try
            {
                var result = context.GstVatAccountList
                    .FromSqlRaw(
                        "EXEC dbo.getGstVatAccountByCompanyId @companyId",
                        new SqlParameter("@companyId", companyId)
                    )
                    .AsNoTracking()
                    .AsEnumerable()   // ✅ required for EXEC
                    .ToList();

                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        [HttpGet("gst-vat-account-by-id/{id}")]
        public IActionResult GetGstVatAccountById(int id)
        {
            try
            {
                var result = context.GstVatAccouts
                    .FromSqlRaw(
                        "EXEC dbo.getGstVatAccountById @id",
                        new SqlParameter("@id", id)
                    )
                    .AsNoTracking()
                    .AsEnumerable()    // ✅ required for EXEC
                    .FirstOrDefault();

                if (result == null)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "GST/VAT account not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }


        [HttpDelete("gst-vat-account-delete/{id}")]
        public async Task<IActionResult> DeleteGstVatAccount(int id)
        {
            try
            {
                var returnParam = new SqlParameter
                {
                    ParameterName = "@ReturnVal",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                await context.Database.ExecuteSqlRawAsync(
                    @"DECLARE @Result INT;
              EXEC @Result = dbo.deleteGstVatAccount @id;
              SET @ReturnVal = @Result;",
                    new SqlParameter("@id", id),
                    returnParam
                );

                int result = (int)(returnParam.Value ?? 0);

                return result switch
                {
                    1 => Ok(new
                    {
                        success = true,
                        message = "GST/VAT account deleted successfully"
                    }),

                    0 => Ok(new
                    {
                        success = false,
                        message = "GST/VAT account not found"
                    }),

                    -1 => Ok(new
                    {
                        success = false,
                        message = "GST/VAT account is used in Account Master and cannot be deleted"
                    }),

                    _ => StatusCode(500, new
                    {
                        success = false,
                        message = "Unexpected result from database"
                    })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to delete GST/VAT account",
                    error = ex.Message
                });
            }
        }


        [HttpPost("sale-heading-add-edit")]
        public IActionResult AddEditSaleHeading([FromBody] SaleHeading model)
        {
            try
            {
                var result = context.CommonSpResponse
                    .FromSqlRaw(
                        @"EXEC dbo.AddEditSaleHeading 
                    @Id, 
                    @CompanyId, 
                    @TypeOfSale, 
                    @Prefix, 
                    @Suffix, 
                    @TaxOnSaleType, 
                    @NumberStartFrom,
                    @Permission",
                        new SqlParameter("@Id", model.Id),
                        new SqlParameter("@CompanyId", model.CompanyId),
                        new SqlParameter("@TypeOfSale", model.TypeOfSale),
                        new SqlParameter("@Prefix", (object?)model.Prefix ?? DBNull.Value),
                        new SqlParameter("@Suffix", (object?)model.Suffix ?? DBNull.Value),
                        new SqlParameter("@TaxOnSaleType", model.TaxOnSaleType),
                        new SqlParameter("@NumberStartFrom", model.NumberStartFrom),
                         new SqlParameter("@Permission", model.Permission)
                    )
                    .AsNoTracking()
                    .AsEnumerable()     // ✅ required for EXEC
                    .FirstOrDefault();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = 0,
                    message = ex.Message
                });
            }
        }

        [HttpGet("sale-heading-by-company/{companyId}")]
        public async Task<IActionResult> GetSaleHeadingByCompanyId(int companyId)
        {
            try
            {
                var companyParam = new SqlParameter("@CompanyId", companyId);

                var data = await context.SaleHeadingDto
                    .FromSqlRaw("EXEC dbo.GetSaleHeadingByCompanyId @CompanyId", companyParam)
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    count = data.Count,
                    data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet("by-id/{id}")]
        public async Task<IActionResult> GetSaleHeadingById(int id)
        {
            try
            {
                var idParam = new SqlParameter("@Id", id);

                var data = context.SaleHeadingById
                    .FromSqlRaw("EXEC dbo.GetSaleHeadingById @Id", idParam)
                    .AsEnumerable()     // 🔥 REQUIRED
                    .FirstOrDefault();

                if (data == null)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "Record not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpDelete("sale-headind-delete/{id}")]
        public async Task<IActionResult> SaleHeadingDelete(int id)
        {
            try
            {
                var idParam = new SqlParameter("@Id", id);

                var resultParam = new SqlParameter("@Result", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                await context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.DeleteSaleHeadingById @Id, @Result OUTPUT",
                    idParam,
                    resultParam
                );

                int result = resultParam.Value != DBNull.Value
                    ? Convert.ToInt32(resultParam.Value)
                    : 0;

                if (result == 1)
                    return Ok(new { success = true, message = "Heading deleted successfully" });

                if (result == 0)
                    return Ok(new { success = false, message = "Heading not found" });

                if (result == -1)
                    return Ok(new { success = false, message = "Cannot delete. Heading is used in Sale Invoice." });

                return StatusCode(500, new { success = false, message = "Unexpected result" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to delete Sale Heading",
                    error = ex.Message
                });
            }
        }


        [HttpPost("purchase-heading-add-edit")]
        public IActionResult AddEditPurchaseHeading([FromBody] purchaseHeading model)
        {
            try
            {
                var result = context.CommonSpResponse
                    .FromSqlRaw(
                        @"EXEC dbo.AddEditPurchaseHeading 
                    @Id, 
                    @CompanyId, 
                    @TypeOfPurchase, 
                    @Prefix, 
                    @Suffix, 
                    @TaxOnPurchaseType, 
                    @NumberStartFrom,
                    @Permission",
                        new SqlParameter("@Id", model.Id),
                        new SqlParameter("@CompanyId", model.CompanyId),
                        new SqlParameter("@TypeOfPurchase", model.TypeOfPurchase),
                        new SqlParameter("@Prefix", (object?)model.Prefix ?? DBNull.Value),
                        new SqlParameter("@Suffix", (object?)model.Suffix ?? DBNull.Value),
                        new SqlParameter("@TaxOnPurchaseType", model.TaxOnPurchaseType),
                        new SqlParameter("@NumberStartFrom", model.NumberStartFrom),
                         new SqlParameter("@Permission", model.Permission)
                    )
                    .AsNoTracking()
                    .AsEnumerable()     // ✅ required for EXEC
                    .FirstOrDefault();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = 0,
                    message = ex.Message
                });
            }
        }


        [HttpGet("pourchase-heading-by-company/{companyId}")]
        public async Task<IActionResult> GetPurchaseHeadingByCompanyId(int companyId)
        {
            try
            {
                var companyParam = new SqlParameter("@CompanyId", companyId);

                var data = await context.purchaseDto
                    .FromSqlRaw("EXEC dbo.GetPurchaseHeadingByCompanyId @CompanyId", companyParam)
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    count = data.Count,
                    data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet("purchase-by-id/{id}")]
        public async Task<IActionResult> GetPurchaseHeadingById(int id)
        {
            try
            {
                var idParam = new SqlParameter("@Id", id);

                var data = context.purchaseById
                    .FromSqlRaw("EXEC dbo.GetPurchaseHeadingById @Id", idParam)
                    .AsEnumerable()     // 🔥 REQUIRED
                    .FirstOrDefault();

                if (data == null)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "Record not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }



        [HttpDelete("purchase-headind-delete/{id}")]
        public async Task<IActionResult> PurchaseHeadingDelete(int id)
        {
            try
            {
                var idParam = new SqlParameter("@Id", id);

                var resultParam = new SqlParameter("@Result", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                await context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.DeletePurchaseHeadingById @Id, @Result OUTPUT",
                    idParam,
                    resultParam
                );

                int result = resultParam.Value != DBNull.Value
                    ? Convert.ToInt32(resultParam.Value)
                    : 0;

                if (result == 1)
                    return Ok(new { success = true, message = "Heading deleted successfully" });

                if (result == 0)
                    return Ok(new { success = false, message = "Heading not found" });

                if (result == -1)
                    return Ok(new { success = false, message = "Cannot delete. Heading is used in Sale Invoice." });

                return StatusCode(500, new { success = false, message = "Unexpected result" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to delete Sale Heading",
                    error = ex.Message
                });
            }
        }

        //[HttpGet("get-next-invoice-no")]
        //public IActionResult GetNextInvoiceNo(
        //  int companyId,
        //  int invoiceHeadingInt,
        //  int startFrom,
        //  string prefix)  // must match stored procedure
        //{
        //    try
        //    {
        //        var result = context.Set<NextInvoiceNoDto>()
        //            .FromSqlRaw(
        //                "EXEC dbo.GetNextInvoiceNo @CompanyId, @InvoiceHeadingInt, @StartFrom, @Prefix",
        //                new SqlParameter("@CompanyId", companyId),
        //                new SqlParameter("@InvoiceHeadingInt", invoiceHeadingInt),
        //                new SqlParameter("@StartFrom", startFrom),
        //                new SqlParameter("@Prefix", prefix ?? "")  // never null
        //            )
        //            .AsEnumerable()
        //            .FirstOrDefault();

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        // This will help debug SQL errors
        //        return BadRequest(new { message = ex.Message, stack = ex.StackTrace });
        //    }
        //}



        [HttpGet("get-next-invoice-no")]
        public IActionResult GetNextInvoiceNo(
    int companyId,
    int invoiceHeadingInt,
    int startFrom,
    string? prefix,
    string? suffix)   // new parameter
        {
            try
            {
                var result = context.Set<NextInvoiceNoDto>()
                    .FromSqlRaw(
                        "EXEC dbo.GetNextInvoiceNo @CompanyId, @InvoiceHeadingInt, @StartFrom, @Prefix, @Suffix",
                        new SqlParameter("@CompanyId", companyId),
                        new SqlParameter("@InvoiceHeadingInt", invoiceHeadingInt),
                        new SqlParameter("@StartFrom", startFrom),
                        new SqlParameter("@Prefix", (object?)prefix ?? DBNull.Value),
                        new SqlParameter("@Suffix", (object?)suffix ?? DBNull.Value)
                    )
                    .AsEnumerable()
                    .FirstOrDefault();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, stack = ex.StackTrace });
            }
        }





        [HttpGet("get-next-Purchaseinvoice-no")]
        public IActionResult GetNextPurchaseInvoiceNo(
      int companyId,
      int invoiceHeadingInt,
      int startFrom,
      string? prefix,
      string? suffix)
        {
            try
            {
                var result = context.Set<NextInvoiceHeadingNoDto>()
                    .FromSqlRaw(
                        "EXEC dbo.GetNextPurchaseInvoiceNo @CompanyId, @InvoiceHeadingInt, @StartFrom, @Prefix, @Suffix",
                        new SqlParameter("@CompanyId", companyId),
                        new SqlParameter("@InvoiceHeadingInt", invoiceHeadingInt),
                        new SqlParameter("@StartFrom", startFrom),
                        new SqlParameter("@Prefix", (object?)prefix ?? DBNull.Value),
                        new SqlParameter("@Suffix", (object?)suffix ?? DBNull.Value)
                    )
                    .AsNoTracking()
                    .AsEnumerable()
                    .FirstOrDefault();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    innerMessage = ex.InnerException?.Message,
                    stack = ex.StackTrace
                });
            }
        }


        [HttpPost("screen-management-save-update")]
        public async Task<IActionResult> SaveOrUpdateScreenManagement(
     [FromBody] ScreenManagement model)
        {
            try
            {
                var list = await context.Database
                    .SqlQueryRaw<CommonSpResponseWithId>(
                        @"EXEC dbo.SaveOrUpdateScreenManagement
                @ScreenId,
                @CompanyId,

                @RemarksSale,
                @HSNSale,
                @ArtSale,
                @SizeSale,
                @ColorSale,
                @Pack1Sale,
                @Pack2Sale,
                @mRateSale,

                @BarcodeSale,
                @DiscPercentSale,
                @DiscountSale,
                @TermAndConditionSale,
                @WhatWeDoInSale,
                @DescriptionSale,

                @RemarksPurchase,
                @HSNPurchase,
                @ArtPurchase,
                @SizePurchase,
                @ColorPurchase,
                @Pack1Purchase,
                @Pack2Purchase,
                @mRatePurchase,

                @BarcodePurchase,
                @DiscPercentPurchase,
                @DiscountPurchase,
                @TermAndConditionPurchase,
                @WhatWeDoInPurchase,
                @DescriptionPurchase",

                        new SqlParameter("@ScreenId", model.ScreenId),
                        new SqlParameter("@CompanyId", model.CompanyId),

                        new SqlParameter("@RemarksSale", (object?)model.RemarksSale ?? DBNull.Value),
                        new SqlParameter("@HSNSale", (object?)model.HSNSale ?? DBNull.Value),
                        new SqlParameter("@ArtSale", (object?)model.ArtSale ?? DBNull.Value),
                        new SqlParameter("@SizeSale", (object?)model.SizeSale ?? DBNull.Value),
                        new SqlParameter("@ColorSale", (object?)model.ColorSale ?? DBNull.Value),
                        new SqlParameter("@Pack1Sale", (object?)model.Pack1Sale ?? DBNull.Value),
                        new SqlParameter("@Pack2Sale", (object?)model.Pack2Sale ?? DBNull.Value),
                        new SqlParameter("@mRateSale", (object?)model.mRateSale ?? DBNull.Value),

                        new SqlParameter("@BarcodeSale", (object?)model.BarcodeSale ?? DBNull.Value),
                        new SqlParameter("@DiscPercentSale", (object?)model.DiscPercentSale ?? DBNull.Value),
                        new SqlParameter("@DiscountSale", (object?)model.DiscountSale ?? DBNull.Value),

                        new SqlParameter("@TermAndConditionSale", (object?)model.TermAndConditionSale ?? DBNull.Value),
                        new SqlParameter("@WhatWeDoInSale", (object?)model.WhatWeDoInSale ?? DBNull.Value),
                        new SqlParameter("@DescriptionSale", (object?)model.DescriptionSale ?? DBNull.Value),

                        new SqlParameter("@RemarksPurchase", (object?)model.RemarksPurchase ?? DBNull.Value),
                        new SqlParameter("@HSNPurchase", (object?)model.HSNPurchase ?? DBNull.Value),
                        new SqlParameter("@ArtPurchase", (object?)model.ArtPurchase ?? DBNull.Value),
                        new SqlParameter("@SizePurchase", (object?)model.SizePurchase ?? DBNull.Value),
                        new SqlParameter("@ColorPurchase", (object?)model.ColorPurchase ?? DBNull.Value),
                        new SqlParameter("@Pack1Purchase", (object?)model.Pack1Purchase ?? DBNull.Value),
                        new SqlParameter("@Pack2Purchase", (object?)model.Pack2Purchase ?? DBNull.Value),
                        new SqlParameter("@mRatePurchase", (object?)model.mRatePurchase ?? DBNull.Value),

                        new SqlParameter("@BarcodePurchase", (object?)model.BarcodePurchase ?? DBNull.Value),
                        new SqlParameter("@DiscPercentPurchase", (object?)model.DiscPercentPurchase ?? DBNull.Value),
                        new SqlParameter("@DiscountPurchase", (object?)model.DiscountPurchase ?? DBNull.Value),

                        new SqlParameter("@TermAndConditionPurchase", (object?)model.TermAndConditionPurchase ?? DBNull.Value),
                        new SqlParameter("@WhatWeDoInPurchase", (object?)model.WhatWeDoInPurchase ?? DBNull.Value),
                        new SqlParameter("@DescriptionPurchase", (object?)model.DescriptionPurchase ?? DBNull.Value)
                    )
                    .ToListAsync();

                var result = list.FirstOrDefault();

                if (result == null)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "Operation failed"
                    });
                }

                return Ok(new
                {
                    success = result.Success == 1,
                    message = result.Message,
                    newId = result.NewId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }

        [HttpGet("screen-management-by-company/{companyId}")]
        public async Task<IActionResult> GetScreenManagementByCompanyId(int companyId)
        {
            try
            {
                var list = await context.Database
                    .SqlQueryRaw<ScreenManagement>(
                        @"EXEC dbo.GetScreenManagementByCompanyId @CompanyId",
                        new SqlParameter("@CompanyId", companyId)
                    )
                    .ToListAsync();   // 🔥 BREAK COMPOSITION

                if (list == null || list.Count == 0)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "No screen management data found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = list
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }


        [HttpGet("check-item-exists/{companyId}/{itemName}/{itemId}")]
        public IActionResult CheckItemExists(
      int companyId,
      string itemName,
      int itemId = 0)
        {
            try
            {
                itemName = itemName?.Trim() ?? string.Empty;

                bool exists = context.TbItemMasters.Any(x =>
                    x.CompanyId == companyId &&
                    x.ItemId != itemId &&
                    x.ItemName != null &&
                    x.ItemName.Trim().ToLower() ==
                    itemName!.ToLower()
                );

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        existsData = exists,
                        message = exists
                            ? "Item already exists"
                            : "Item available"
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }


        [HttpPost("add-edit-item-master")]
        public async Task<IActionResult> AddEditItemMaster([FromBody] TbItemMaster model)
        {
            try
            {
                var parameters = new[]
                {
            new SqlParameter("@itemId", model.ItemId),
            new SqlParameter("@companyId", model.CompanyId),
            new SqlParameter("@description", model.Description ?? (object)DBNull.Value),
            new SqlParameter("@itemName", model.ItemName),
            new SqlParameter("@unit", model.Unit),
            new SqlParameter("@unitInt", model.UnitInt ?? (object)DBNull.Value),
             new SqlParameter("@categoryId", model.CategoryId ?? (object)DBNull.Value),
            new SqlParameter("@saleRate", model.SaleRate ?? (object)DBNull.Value),
            new SqlParameter("@purchaseRate", model.PurchaseRate ?? (object)DBNull.Value),
            new SqlParameter("@mrpRate", model.MrpRate ?? (object)DBNull.Value),
            new SqlParameter("@hsn", model.Hsn),
            new SqlParameter("@itemType", model.ItemType),
            new SqlParameter("@openingStock", model.OpeningStock ?? (object)DBNull.Value),
            new SqlParameter("@openingStockRate", model.OpeningStockRate ?? (object)DBNull.Value),
            new SqlParameter("@openingStockValue", model.OpeningStockValue ?? (object)DBNull.Value),
            new SqlParameter("@discount", model.Discount ?? (object)DBNull.Value),
            new SqlParameter("@total", model.Total ?? (object)DBNull.Value),
            new SqlParameter("@packingInUnit", model.PackingInUnit ?? (object)DBNull.Value),
            new SqlParameter("@packing", model.Packing ?? (object)DBNull.Value),
            new SqlParameter("@barCodeType", model.BarCodeType ?? (object)DBNull.Value),
            new SqlParameter("@itemBarCodeOrPartNo", model.ItemBarCodeOrPartNo ?? (object)DBNull.Value),
            new SqlParameter("@enterBy", model.EnterBy),
            new SqlParameter("@cgstSgstSale", SqlDbType.Int){ Value = model.CgstSgstSale ?? (object)DBNull.Value},
            new SqlParameter("@igstSaleName", SqlDbType.Int){Value = model.IgstSaleName ?? (object)DBNull.Value},
            new SqlParameter("@cgstSgstPurchase", SqlDbType.Int){Value = model.CgstSgstPurchase ?? (object)DBNull.Value},
            new SqlParameter("@igstPurchase", SqlDbType.Int){Value = model.IgstPurchase ?? (object)DBNull.Value},
            new SqlParameter("@taxRate", model.TaxRate ?? (object)DBNull.Value),
            new SqlParameter("@cessRate", model.CessRate ?? (object)DBNull.Value),
            new SqlParameter("@cessQty", model.CessQty ?? (object)DBNull.Value),
            new SqlParameter("@itemGroupId", model.ItemGroupId ?? (object)DBNull.Value),};

                await context.Database.ExecuteSqlRawAsync(
                    @"EXEC dbo.addEditItemMaster
              @itemId, @companyId,@categoryId, @description, @itemName, @unit, @unitInt,
              @saleRate,@purchaseRate, @mrpRate, @hsn, @itemType,
              @openingStock, @openingStockRate, @openingStockValue,
              @discount, @total, @packingInUnit, @packing,
              @barCodeType, @itemBarCodeOrPartNo, @enterBy,
              @cgstSgstSale, @igstSaleName, @cgstSgstPurchase, @igstPurchase,
              @taxRate, @cessRate, @cessQty, @itemGroupId",
                    parameters
                );

                return Ok(new
                {
                    success = true,
                    message = model.ItemId > 0
                        ? "Item updated successfully"
                        : "Item added successfully"
                });
            }
            catch (SqlException ex)
            {
                // Duplicate item name error from RAISERROR
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error",
                    error = ex.Message
                });
            }
        }

        //[HttpGet("get-all-item-master/{companyId}")]
        //public async Task<IActionResult> GetAllItemMaster(int companyId)
        //{
        //    try
        //    {
        //        var companyIdParam = new SqlParameter("@companyId", companyId);

        //        var data = await context.Set<ItemMasterListDto>()
        //            .FromSqlRaw(
        //                "EXEC dbo.getAllItemMasterData @companyId",
        //                companyIdParam)
        //            .AsNoTracking()
        //            .ToListAsync();

        //        return Ok(new
        //        {
        //            success = true,
        //            count = data.Count,
        //            data
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new
        //        {
        //            success = false,
        //            message = "Internal server error",
        //            error = ex.Message
        //        });
        //    }
        //}

        [HttpGet("get-all-item-master/{companyId}")]
        public async Task<IActionResult> GetAllItemMaster(int companyId)
        {
            try
            {
                var companyIdParam = new SqlParameter("@companyId", companyId);

                var data = await context.Set<ItemMasterListDto>()
                    .FromSqlRaw(
                        "EXEC dbo.getAllItemMasterData @companyId",
                        companyIdParam)
                    .AsNoTracking()
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    count = data.Count,
                    data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error",
                    error = ex.Message
                });
            }
        }


        [HttpGet("get-item-by-id/{itemId}")]
        public async Task<IActionResult> GetItemById(int itemId)
        {
            try
            {
                var itemIdParam = new SqlParameter("@itemId", itemId);

                var item = context.Set<TbItemMaster>()
                    .FromSqlRaw("EXEC dbo.getItemById @itemId", itemIdParam)
                    .AsNoTracking()
                    .AsEnumerable()          // ✅ IMPORTANT
                    .FirstOrDefault();       // client-side now

                if (item == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Item not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = item
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("delete-item-by-id/{itemId}")]
        public async Task<IActionResult> DeleteItemById(int itemId)
        {
            try
            {
                var itemIdParam = new SqlParameter("@itemId", itemId);

                var resultParam = new SqlParameter("@Result", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                await context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.deleteItemById @itemId, @Result OUTPUT",
                    itemIdParam,
                    resultParam
                );

                int result = Convert.ToInt32(resultParam.Value);

                return result switch
                {
                    1 => Ok(new
                    {
                        success = true,
                        message = "Item deleted successfully"
                    }),

                    -2 => Ok(new
                    {
                        success = false,
                        message = "Item cannot be deleted. It is used in Sale Invoice."
                    }),

                    -1 => NotFound(new
                    {
                        success = false,
                        message = "Item not found"
                    }),

                    _ => StatusCode(500, new
                    {
                        success = false,
                        message = "Unexpected result"
                    })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error",
                    error = ex.Message
                });
            }
        }


        [HttpGet("get-item-wise-stock")]
        public async Task<IActionResult> GetItemWiseStock(
      int companyId,
      int? itemId = null,
      DateTime? fromDate = null,
      DateTime? toDate = null)
        {
            try
            {
                var stock = new List<ItemWiseStockDto>();
                var transactions = new List<ItemWiseStockTransactionDto>();

                using var conn = context.Database.GetDbConnection();

                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();

                cmd.CommandText = "dbo.sp_GetItemWiseStock";
                cmd.CommandType = CommandType.StoredProcedure;

                // =====================================================
                // COMPANY ID
                // =====================================================

                var companyParam = cmd.CreateParameter();
                companyParam.ParameterName = "@CompanyId";
                companyParam.Value = companyId;
                cmd.Parameters.Add(companyParam);


                // =====================================================
                // ITEM ID
                // =====================================================

                var itemParam = cmd.CreateParameter();
                itemParam.ParameterName = "@ItemId";
                itemParam.Value = itemId.HasValue
                    ? itemId.Value
                    : DBNull.Value;

                cmd.Parameters.Add(itemParam);


                // =====================================================
                // FROM DATE
                // =====================================================

                var fromDateParam = cmd.CreateParameter();
                fromDateParam.ParameterName = "@FromDate";
                fromDateParam.Value = fromDate.HasValue
                    ? fromDate.Value.Date
                    : DBNull.Value;

                cmd.Parameters.Add(fromDateParam);


                // =====================================================
                // TO DATE
                // =====================================================

                var toDateParam = cmd.CreateParameter();
                toDateParam.ParameterName = "@ToDate";
                toDateParam.Value = toDate.HasValue
                    ? toDate.Value.Date
                    : DBNull.Value;

                cmd.Parameters.Add(toDateParam);


                // =====================================================
                // EXECUTE STORED PROCEDURE
                // =====================================================

                using var reader = await cmd.ExecuteReaderAsync();


                // =====================================================
                // RESULT SET 1
                // ITEM-WISE STOCK SUMMARY
                // =====================================================

                while (await reader.ReadAsync())
                {
                    stock.Add(new ItemWiseStockDto
                    {
                        ItemId = reader["ItemId"] == DBNull.Value
                            ? 0
                            : Convert.ToInt32(reader["ItemId"]),

                        ItemName = reader["ItemName"] == DBNull.Value
                            ? null
                            : reader["ItemName"].ToString(),

                        Description = reader["Description"] == DBNull.Value
                            ? null
                            : reader["Description"].ToString(),

                        Unit = reader["Unit"] == DBNull.Value
                            ? null
                            : reader["Unit"].ToString(),

                        Hsn = reader["Hsn"] == DBNull.Value
                            ? null
                            : reader["Hsn"].ToString(),

                        MasterOpeningQty = reader["MasterOpeningQty"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(reader["MasterOpeningQty"]),

                        PreviousPurchaseQty = reader["PreviousPurchaseQty"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(reader["PreviousPurchaseQty"]),

                        PreviousSaleQty = reader["PreviousSaleQty"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(reader["PreviousSaleQty"]),

                        OpeningQty = reader["OpeningQty"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(reader["OpeningQty"]),

                        PurchaseQty = reader["PurchaseQty"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(reader["PurchaseQty"]),

                        SaleQty = reader["SaleQty"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(reader["SaleQty"]),

                        NetQty = reader["NetQty"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(reader["NetQty"]),

                        ClosingQty = reader["ClosingQty"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(reader["ClosingQty"]),

                        PurchaseValue = reader["PurchaseValue"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(reader["PurchaseValue"]),

                        SaleValue = reader["SaleValue"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(reader["SaleValue"])
                    });
                }


                // =====================================================
                // MOVE TO RESULT SET 2
                // PURCHASE + SALE TRANSACTIONS
                // =====================================================

                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        transactions.Add(new ItemWiseStockTransactionDto
                        {
                            TransactionType =
                                reader["TransactionType"] == DBNull.Value
                                    ? null
                                    : reader["TransactionType"].ToString(),

                            ItemId =
                                reader["ItemId"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(reader["ItemId"]),

                            ItemName =
                                reader["ItemName"] == DBNull.Value
                                    ? null
                                    : reader["ItemName"].ToString(),

                            InvoiceId =
                                reader["InvoiceId"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(reader["InvoiceId"]),

                            InvoiceNo =
                                reader["InvoiceNo"] == DBNull.Value
                                    ? null
                                    : reader["InvoiceNo"].ToString(),

                            InvoiceDate =
                                reader["InvoiceDate"] == DBNull.Value
                                    ? null
                                    : Convert.ToDateTime(reader["InvoiceDate"]),

                            Qty =
                                reader["Qty"] == DBNull.Value
                                    ? 0
                                    : Convert.ToDecimal(reader["Qty"]),

                            Rate =
                                reader["Rate"] == DBNull.Value
                                    ? 0
                                    : Convert.ToDecimal(reader["Rate"]),

                            Amount =
                                reader["Amount"] == DBNull.Value
                                    ? 0
                                    : Convert.ToDecimal(reader["Amount"])
                        });
                    }
                }


                // =====================================================
                // FINAL RESPONSE
                // =====================================================

                return Ok(new
                {
                    success = true,

                    companyId = companyId,

                    itemId = itemId,

                    fromDate = fromDate,

                    toDate = toDate,

                    count = stock.Count,

                    transactionCount = transactions.Count,

                    data = stock,

                    transactions = transactions
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch item wise stock",
                    error = ex.Message
                });
            }
        }

        [HttpPost("regdtype")]
        public async Task<IActionResult> AddEditRegdType([FromBody] RegdTypeModel model)
        {
            try
            {
                using var conn = context.Database.GetDbConnection();
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "dbo.AddEditRegdType";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Id", model.Id));
                cmd.Parameters.Add(new SqlParameter("@CompanyId", model.CompanyId));
                cmd.Parameters.Add(new SqlParameter("@RegisterType", model.RegisterType));

                using var reader = await cmd.ExecuteReaderAsync();

                int responseCode = 0;
                string responseMessage = "";

                if (await reader.ReadAsync())
                {
                    responseCode = Convert.ToInt32(reader["ResponseCode"]);
                    responseMessage = Convert.ToString(reader["ResponseMessage"]) ?? "";
                }

                return Ok(new
                {
                    success = responseCode == 1,
                    responseCode = responseCode,
                    message = responseMessage
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    responseCode = 0,
                    message = ex.Message
                });
            }
        }

        [HttpGet("regdtype/company/{companyId}")]
        public async Task<IActionResult> GetRegdTypeByCompanyId(int companyId)
        {
            try
            {
                var regdTypes = new List<RegdTypeModel>();

                using (var conn = context.Database.GetDbConnection())
                {
                    await conn.OpenAsync();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "dbo.GetRegdTypeByCompanyId";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        var param = cmd.CreateParameter();
                        param.ParameterName = "@CompanyId";
                        param.Value = companyId;
                        cmd.Parameters.Add(param);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                regdTypes.Add(new RegdTypeModel
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    CompanyId = Convert.ToInt32(reader["CompanyId"]),
                                    RegisterType = Convert.ToString(reader["RegisterType"]) ?? ""
                                });
                            }
                        }
                    }
                }

                return Ok(new
                {
                    success = true,
                    count = regdTypes.Count,
                    data = regdTypes
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch Register Types.",
                    error = ex.Message
                });
            }
        }

        [HttpGet("regdtype/{id}")]
        public async Task<IActionResult> GetRegdTypeById(int id)
        {
            try
            {
                RegdTypeModel? regdType = null;

                using (var conn = context.Database.GetDbConnection())
                {
                    await conn.OpenAsync();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "dbo.GetRegdTypeById";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        var param = cmd.CreateParameter();
                        param.ParameterName = "@Id";
                        param.Value = id;
                        cmd.Parameters.Add(param);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                regdType = new RegdTypeModel
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    CompanyId = Convert.ToInt32(reader["CompanyId"]),
                                    RegisterType = Convert.ToString(reader["RegisterType"]) ?? ""
                                };
                            }
                        }
                    }
                }

                return Ok(new
                {
                    success = true,
                    data = regdType
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }


        [HttpDelete("regdtype/{id}")]
        public async Task<IActionResult> DeleteRegdTypeById(int id)
        {
            try
            {
                int responseCode = 0;
                string responseMessage = "";

                using (var conn = context.Database.GetDbConnection())
                {
                    await conn.OpenAsync();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "dbo.DeleteRegdTypeById";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        var param = cmd.CreateParameter();
                        param.ParameterName = "@Id";
                        param.Value = id;
                        cmd.Parameters.Add(param);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                responseCode = Convert.ToInt32(reader["ResponseCode"]);
                                responseMessage = Convert.ToString(reader["ResponseMessage"]) ?? "";
                            }
                        }
                    }
                }

                return Ok(new
                {
                    success = responseCode == 1,
                    responseCode,
                    message = responseMessage
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    responseCode = -1,
                    message = ex.Message
                });
            }
        }
        [HttpPost("add-edit-account-master")]
        public async Task<IActionResult> AddEditAccountMaster([FromBody] TbAccountMaster model)
        {
            try
            {
                var sqlParams = new[]
                {
            new SqlParameter("@id", model.Id),
            new SqlParameter("@companyId", model.CompanyId),
            new SqlParameter("@groupId", (object?)model.GroupId ?? DBNull.Value),
            new SqlParameter("@stateUser", (object?)model.StateUser ?? DBNull.Value),
            new SqlParameter("@transportId", (object?)model.TransportId ?? DBNull.Value),
            new SqlParameter("@accountName", model.AccountName),

            new SqlParameter("@address", (object?)model.Address ?? DBNull.Value),
            new SqlParameter("@zipCode", (object?)model.ZipCode ?? DBNull.Value),
            new SqlParameter("@email", (object?)model.Email ?? DBNull.Value),
            new SqlParameter("@phone", (object?)model.Phone ?? DBNull.Value),

            new SqlParameter("@gst", (object?)model.Gst ?? DBNull.Value),
            new SqlParameter("@pan", (object?)model.Pan ?? DBNull.Value),
            new SqlParameter("@adharNo", (object?)model.AdharNo ?? DBNull.Value),

            new SqlParameter("@contectName", (object?)model.ContectName ?? DBNull.Value),
            new SqlParameter("@contectNo", (object?)model.ContectNo ?? DBNull.Value),

            new SqlParameter("@bankName", (object?)model.BankName ?? DBNull.Value),
            new SqlParameter("@ifscCode", (object?)model.IfscCode ?? DBNull.Value),

            new SqlParameter("@creditDay", (object?)model.CreditDay ?? DBNull.Value),
            new SqlParameter("@regdType", (object?)model.RegdType ?? DBNull.Value),

            new SqlParameter("@registrationTypeId",(object?)model.RegistrationTypeId ?? DBNull.Value),


            new SqlParameter("@isTcsCompulsary", (object?)model.IsTcsCompulsary ?? DBNull.Value),
            new SqlParameter("@tdsApplicabe", (object?)model.TdsApplicabe ?? DBNull.Value),
            new SqlParameter("@isItTransport", (object?)model.IsItTransport ?? DBNull.Value),

            new SqlParameter("@transportMode", (object?)model.TransportMode ?? DBNull.Value),
            new SqlParameter("@transportRate", (object?)model.TransportRate ?? DBNull.Value),
            new SqlParameter("@tcsLimit", (object?)model.TcsLimit ?? DBNull.Value),

            new SqlParameter("@cityId", (object?)model.CityId ?? DBNull.Value),
            new SqlParameter("@tanNo", (object?)model.TanNo ?? DBNull.Value),

            new SqlParameter("@opBalance", (object?)model.OpBalance ?? DBNull.Value),
            new SqlParameter("@openingBalanceType", (object?)model.OpeningBalanceType ?? DBNull.Value),
            new SqlParameter("@clsBalance", (object?)model.ClsBalance ?? DBNull.Value),
            new SqlParameter("@closingBalanceType", (object?)model.ClosingBalanceType ?? DBNull.Value),

            new SqlParameter("@agentAndAreaName", (object?)model.AgentAndAreaName ?? DBNull.Value),
            new SqlParameter("@openingBalance", (object?)model.OpeningBalance ?? DBNull.Value),
            new SqlParameter("@closingBalance", (object?)model.ClosingBalance ?? DBNull.Value),

            new SqlParameter("@state", (object?)model.State ?? DBNull.Value),
            new SqlParameter("@gstVatReturn", (object?)model.GstVatReturn ?? DBNull.Value),
            new SqlParameter("@maintBillWise", (object?)model.MaintBillWise ?? DBNull.Value),

            new SqlParameter("@eComNo", (object?)model.EComNo ?? DBNull.Value),
            new SqlParameter("@expHsnCode", (object?)model.ExpHsnCode ?? DBNull.Value),
            new SqlParameter("@cinNo", (object?)model.CinNo ?? DBNull.Value),
            new SqlParameter("@ieCodeNo", (object?)model.IeCodeNo ?? DBNull.Value),

            new SqlParameter("@creditLimit", (object?)model.CreditLimit ?? DBNull.Value),
            new SqlParameter("@inttRate", (object?)model.InttRate ?? DBNull.Value),
            new SqlParameter("@basicLimit", (object?)model.BasicLimit ?? DBNull.Value),

            new SqlParameter("@taxFormName", (object?)model.TaxFormName ?? DBNull.Value),
            new SqlParameter("@tradeType", (object?)model.TradeType ?? DBNull.Value),
             new SqlParameter("@comments", (object?)model.Comments ?? DBNull.Value),

            new SqlParameter("@agentId", (object?)model.AgentId ?? DBNull.Value),
            new SqlParameter("@gstVat", model.GstVat)
        };

                await context.Database.ExecuteSqlRawAsync(
                    @"EXEC dbo.sp_AddEdit_AccountMaster
              @id, @companyId, @groupId, @stateUser, @transportId, @accountName,
              @address, @zipCode, @email, @phone, @gst, @pan, @adharNo,
              @contectName, @contectNo, @bankName, @ifscCode, @creditDay, @regdType,
              @registrationTypeId,
              @isTcsCompulsary, @tdsApplicabe, @isItTransport,
              @transportMode, @transportRate, @tcsLimit,
              @cityId, @tanNo, @opBalance, @openingBalanceType,
              @clsBalance, @closingBalanceType,
              @agentAndAreaName, @openingBalance, @closingBalance,
              @state, @gstVatReturn, @maintBillWise,
              @eComNo, @expHsnCode, @cinNo, @ieCodeNo,
              @creditLimit, @inttRate, @basicLimit,
              @taxFormName, @tradeType,@comments, @agentId, @gstVat",
                    sqlParams
                );

                return Ok(new
                {
                    success = true,
                    message = model.Id == 0 ? "Account added successfully" : "Account updated successfully"
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error",
                    error = ex.Message
                });
            }
        }



        [HttpGet("check-account-exists/{companyId}/{accountName}/{id}")]
        public async Task<IActionResult> CheckAccountExists(
          int companyId,
          string accountName,
          int id = 0)
        {
            try
            {
                var companyParam = new SqlParameter("@CompanyId", companyId);
                var accountParam = new SqlParameter("@AccountName", accountName);
                var idParam = new SqlParameter("@Id", id);

                var data = await context.AccountDuplicateCheckDto
                    .FromSqlRaw(
                        "EXEC dbo.checkAccountAlreadyExists @CompanyId, @AccountName, @Id",
                        companyParam,
                        accountParam,
                        idParam
                    )
                    .AsNoTracking()
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    data = data.FirstOrDefault()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error",
                    error = ex.Message
                });
            }
        }




        [HttpGet("get-accounts-by-company/{companyId}")]
        public async Task<IActionResult> GetAccountsByCompanyId(int companyId)
        {
            try
            {
                var param = new SqlParameter("@companyId", companyId);

                var data = await context.AccountListDto
                    .FromSqlRaw("EXEC dbo.sp_GetAccountsByCompanyId @companyId", param)
                    .AsNoTracking()
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    count = data.Count,
                    data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error",
                    error = ex.Message
                });
            }
        }

        [HttpGet("get-account-by-id/{id}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            try
            {
                var param = new SqlParameter("@id", id);

                var data = context.AccountByIdDto
                    .FromSqlRaw("EXEC dbo.sp_GetAccountById @id", param)
                    .AsNoTracking()
                    .AsEnumerable()        // ✅ KEY LINE
                    .FirstOrDefault();     // ✅ now safe

                if (data == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Account not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("delete-account-by-id/{id}")]
        public async Task<IActionResult> DeleteAccountById(int id)
        {
            try
            {
                var param = new SqlParameter("@id", id);

                var result = context.DbMessageDto
                    .FromSqlRaw("EXEC dbo.sp_DeleteAccountById @id", param)
                    .AsNoTracking()
                    .AsEnumerable()       // ✅ REQUIRED (non-composable SQL)
                    .FirstOrDefault();

                return Ok(new
                {
                    success = true,
                    message = result?.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error",
                    error = ex.Message
                });
            }
        }

        [HttpPost("save-or-update-tax-table")]
        public async Task<IActionResult> SaveOrUpdateTaxTable(
           [FromBody] TaxTableMaster model)
        {
            try
            {
                var parameters = new[]
                {
            new SqlParameter("@id", model.Id == 0 ? (object)DBNull.Value : model.Id),
            new SqlParameter("@companyId", model.CompanyId),
            new SqlParameter("@salePurcAccountName", model.SalePurcAccountName ?? (object)DBNull.Value),
            new SqlParameter("@selectType", model.SelectType ?? (object)DBNull.Value),
            new SqlParameter("@underVatReturn", model.UnderVatReturn ?? (object)DBNull.Value),
            new SqlParameter("@exciseApplicabe", model.ExciseApplicabe ?? (object)DBNull.Value),

            new SqlParameter("@gstApplicabeCentral", model.GstApplicabeCentral ?? (object)DBNull.Value),
            new SqlParameter("@gstApplicabeCentralName", model.GstApplicabeCentralName ?? (object)DBNull.Value),
            new SqlParameter("@gstApplicabeCentralRate", model.GstApplicabeCentralRate ?? (object)DBNull.Value),
            new SqlParameter("@gstApplicabeCentralCalculateOn",
                model.GstApplicabeCentralCalculateOn ?? (object)DBNull.Value),

            new SqlParameter("@gstApplicabeLocal", model.GstApplicabeLocal ?? (object)DBNull.Value),
            new SqlParameter("@gstApplicabeLocalName", model.GstApplicabeLocalName ?? (object)DBNull.Value),
            new SqlParameter("@gstApplicabeLocalRate", model.GstApplicabeLocalRate ?? (object)DBNull.Value),
            new SqlParameter("@gstApplicabeLocalCalculateOn",
                model.GstApplicabeLocalCalculateOn ?? (object)DBNull.Value),

            new SqlParameter("@tcsApplicabe", model.TcsApplicabe ?? (object)DBNull.Value),
            new SqlParameter("@tcsApplicabeName", model.TcsApplicabeName ?? (object)DBNull.Value),
            new SqlParameter("@tcsApplicabeRate", model.TcsApplicabeRate ?? (object)DBNull.Value),
            new SqlParameter("@tcsApplicabeCalculateOn",
                model.TcsApplicabeCalculateOn ?? (object)DBNull.Value),

            new SqlParameter("@swachBhartApplicable", model.SwachBhartApplicable ?? (object)DBNull.Value),
            new SqlParameter("@swachBhartApplicableName", model.SwachBhartApplicableName ?? (object)DBNull.Value),
            new SqlParameter("@swachBhartApplicableRate", model.SwachBhartApplicableRate ?? (object)DBNull.Value),
            new SqlParameter("@swachBhartApplicableCalculateOn",
                model.SwachBhartApplicableCalculateOn ?? (object)DBNull.Value),

            new SqlParameter("@totalax", model.Totalax ?? (object)DBNull.Value),
            new SqlParameter("@SubTotalTax", model.SubTotalTax ?? (object)DBNull.Value)
        };

                await context.Database.ExecuteSqlRawAsync(
                    "EXEC dharmesh.SaveOrUpdateTaxTable " +
                    "@id,@companyId,@salePurcAccountName,@selectType,@underVatReturn,@exciseApplicabe," +
                    "@gstApplicabeCentral,@gstApplicabeCentralName,@gstApplicabeCentralRate,@gstApplicabeCentralCalculateOn," +
                    "@gstApplicabeLocal,@gstApplicabeLocalName,@gstApplicabeLocalRate,@gstApplicabeLocalCalculateOn," +
                    "@tcsApplicabe,@tcsApplicabeName,@tcsApplicabeRate,@tcsApplicabeCalculateOn," +
                    "@swachBhartApplicable,@swachBhartApplicableName,@swachBhartApplicableRate,@swachBhartApplicableCalculateOn," +
                    "@totalax,@SubTotalTax",
                    parameters);

                return Ok(new
                {
                    success = true,
                    message = model.Id == 0
                        ? "Tax record saved successfully"
                        : "Tax record updated successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error",
                    error = ex.Message
                });
            }
        }


        [HttpGet("get-tax-table-by-company/{companyId}")]
        public IActionResult GetTaxTableByCompanyId(int companyId)
        {
            try
            {
                var companyIdParam = new SqlParameter("@companyId", companyId);

                var data = context.TaxTableDetailsDto
                    .FromSqlRaw(
                        "EXEC GetTaxTableDetailsByCompanyId @companyId",
                        companyIdParam
                    )
                    .AsEnumerable()   // prevents non-composable SQL error
                    .ToList();

                return Ok(new
                {
                    success = true,
                    count = data.Count,
                    data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error",
                    error = ex.Message
                });
            }
        }
        [HttpGet("get-tax-table-by-id/{id}")]
        public IActionResult GetTaxTableById(int id)
        {
            try
            {
                var idParam = new SqlParameter("@id", id);

                var data = context.TaxTableMasters
                    .FromSqlRaw(
                        "EXEC GetTaxTableByIdSimple @id",
                        idParam
                    )
                    .AsEnumerable()   // 🔥 REQUIRED for stored procedures
                    .FirstOrDefault();

                if (data == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Tax record not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error",
                    error = ex.Message
                });
            }
        }


        [HttpDelete("delete-tax-table/{id}")]
        public async Task<IActionResult> DeleteTaxTableById(int id)
        {
            try
            {
                var idParam = new SqlParameter("@id", id);

                var statusParam = new SqlParameter("@status", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };

                await context.Database.ExecuteSqlRawAsync(
                    "EXEC DeleteTaxTableById @id, @status OUTPUT",
                    idParam,
                    statusParam
                );

                bool status = statusParam.Value != DBNull.Value && (bool)statusParam.Value;

                if (!status)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "Used In Sale Invoice"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Tax record deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error",
                    error = ex.Message
                });
            }
        }
        [HttpPost("add-edit-Sale")]
        public async Task<IActionResult> AddEditSaleInvoice([FromBody] SaleInvoiceRequest model)
        {
            try
            {
                // SAFETY: ensure JSON array
                var detailsJson = model.SaleInvoiceDetails?.Any() == true
    ? JsonConvert.SerializeObject(model.SaleInvoiceDetails)
    : "[]";

                var parameters = new[]
                {
            new SqlParameter("@SaleInvoiceId", model.SaleInvoiceId ?? (object)DBNull.Value),
            new SqlParameter("@InvoiceHeading", model.InvoiceHeading ?? (object)DBNull.Value),
            new SqlParameter("@InvoiceHeadingInt", model.InvoiceHeadingInt ?? (object)DBNull.Value),
           
            new SqlParameter("@CompanyId", model.CompanyId),
            new SqlParameter("@InvoiceNo", model.InvoiceNo ?? (object)DBNull.Value),
            new SqlParameter("@InvoiceDate", model.InvoiceDate),
            new SqlParameter("@ClaimDate", model.ClaimDate ?? (object)DBNull.Value),
            new SqlParameter("@AccountId", model.AccountId),
             new SqlParameter("@ShipTo", model.ShipTo),
            new SqlParameter("@Transport", model.Transport ?? (object)DBNull.Value),
            new SqlParameter("@TransportNameManual", model.TransportNameManual ?? (object)DBNull.Value),
            new SqlParameter("@ShippingBillNo", model.ShippingBillNo ?? (object)DBNull.Value),
            new SqlParameter("@GRNo", model.GRNo ?? (object)DBNull.Value),
            new SqlParameter("@OrderNo", model.OrderNo ?? (object)DBNull.Value),
            new SqlParameter("@Vehicle", model.Vehicle ?? (object)DBNull.Value),
            new SqlParameter("@FormNo", model.FormNo ?? (object)DBNull.Value),
            new SqlParameter("@Weight", model.Weight ?? (object)DBNull.Value),
            new SqlParameter("@CreditDays", model.CreditDays ?? (object)DBNull.Value),
            new SqlParameter("@PackingNo", model.PackingNo ?? (object)DBNull.Value),
            new SqlParameter("@DocuThru", model.DocuThru ?? (object)DBNull.Value),
            new SqlParameter("@Station", model.Station ?? (object)DBNull.Value),
            new SqlParameter("@RGPNo", model.RGPNo ?? (object)DBNull.Value),
            new SqlParameter("@Dated", model.Dated ?? (object)DBNull.Value),
            new SqlParameter("@Freight", model.Freight ?? (object)DBNull.Value),
            new SqlParameter("@Packages", model.Packages ?? (object)DBNull.Value),
            new SqlParameter("@PvtMark", model.PvtMark ?? (object)DBNull.Value),
            new SqlParameter("@DueDate", model.DueDate ?? (object)DBNull.Value),
            new SqlParameter("@EcomGSTIN", model.EcomGSTIN ?? (object)DBNull.Value),
            new SqlParameter("@EwayNo", model.EwayNo ?? (object)DBNull.Value),
            new SqlParameter("@ShBNo", model.ShBNo ?? (object)DBNull.Value),
            new SqlParameter("@ShipDate", model.ShipDate ?? (object)DBNull.Value),
            new SqlParameter("@ShipPartNo", model.ShipPartNo ?? (object)DBNull.Value),
            new SqlParameter("@PortLoading", model.PortLoading ?? (object)DBNull.Value),
            new SqlParameter("@PortDischarge", model.PortDischarge ?? (object)DBNull.Value),
            new SqlParameter("@FinalDestination", model.FinalDestination ?? (object)DBNull.Value),
            new SqlParameter("@EntrBy", model.EntrBy ?? (object)DBNull.Value),

            new SqlParameter("@SubTotal", model.SubTotal ?? (object)DBNull.Value),
            new SqlParameter("@RoundAndTotal", model.RoundAndTotal ?? (object)DBNull.Value),
            new SqlParameter("@TaxableSale", model.TaxableSale ?? (object)DBNull.Value),
            new SqlParameter("@CentralGst", model.CentralGst ?? (object)DBNull.Value),
            new SqlParameter("@LocalGst", model.LocalGst ?? (object)DBNull.Value),
            new SqlParameter("@Tcs", model.Tcs ?? (object)DBNull.Value),
            new SqlParameter("@SwachBharat", model.SwachBharat ?? (object)DBNull.Value),
            new SqlParameter("@Value", model.Value ?? (object)DBNull.Value),
            new SqlParameter("@OtherCharge", model.OtherCharge),
            new SqlParameter("@Value1", model.Value1 ?? (object)DBNull.Value),
            new SqlParameter("@OtherCharge1", model.OtherCharge1),
            new SqlParameter("@ExtraAmount", model.ExtraAmount),

            // ✅ MOST IMPORTANT FIX
            new SqlParameter("@SaleInvoiceDetails", SqlDbType.NVarChar, -1)
            {
                Value = detailsJson
            }
        };

                var result = await context
                    .Set<ApiResponse>()
                    .FromSqlRaw(
                        @"EXEC dbo.sp_AddOrUpdate_SaleInvoice
                  @SaleInvoiceId,@InvoiceHeading,@InvoiceHeadingInt,@CompanyId,@InvoiceNo,
                  @InvoiceDate,@ClaimDate,@AccountId,@ShipTo,@Transport,@TransportNameManual,@ShippingBillNo,@GRNo,
                  @OrderNo,@Vehicle,@FormNo,@Weight,@CreditDays,@PackingNo,@DocuThru,
                  @Station,@RGPNo,@Dated,@Freight,@Packages,@PvtMark,@DueDate,
                  @EcomGSTIN,@EwayNo,@ShBNo,@ShipDate,@ShipPartNo,@PortLoading,
                  @PortDischarge,@FinalDestination,@EntrBy,@SubTotal,@RoundAndTotal,
                  @TaxableSale,@CentralGst,@LocalGst,@Tcs,@SwachBharat,@Value,
                  @OtherCharge,@Value1,@OtherCharge1,@ExtraAmount,@SaleInvoiceDetails",
                        parameters)
                    .ToListAsync();

                return Ok(result.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }



        [HttpGet("sale-invoices/{companyId}")]
        public async Task<IActionResult> GetSaleInvoicesByCompanyId(int companyId)
        {
            var invoices = new Dictionary<int, SaleInvoiceDto>();

            using var conn = context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "GetSaleInvoicesByCompanyId";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@CompanyId", companyId));

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                int invoiceId = reader.Get<int>("SaleInvoiceId");

                if (!invoices.ContainsKey(invoiceId))
                {
                    invoices[invoiceId] = new SaleInvoiceDto
                    {
                        SaleInvoiceId = invoiceId,
                        InvoiceHeading = reader.Get<string>("InvoiceHeading"),
                        InvoiceHeadingInt = reader.Get<int>("InvoiceHeadingInt"),
                        CompanyId = reader.Get<int>("CompanyId"),
                        InvoiceNo = reader.Get<string>("InvoiceNo"),
                        InvoiceDate = reader.Get<DateTime>("InvoiceDate"),
                        ClaimDate = reader.Get<DateTime>("ClaimDate"),
                        AccountId = reader.Get<int>("AccountId"),
                        AccountName = reader.Get<string>("accountName"),
                        ShipTo = reader.Get<int>("ShipTo"),
                        ShipToName = reader.Get<string>("ShipToName"),
                        TransportNameManual = reader.Get<string>("TransportNameManual"),
                        PvtMark = reader.Get<string>("PvtMark"),
                        DueDate = reader.Get<string>("DueDate"),
                        SubTotal = reader.Get<decimal>("SubTotal"),
                        RoundAndTotal = reader.Get<decimal>("RoundAndTotal"),
                        TaxableSale = reader.Get<decimal>("TaxableSale"),
                        CentralGst = reader.Get<decimal>("CentralGst"),
                        LocalGst = reader.Get<decimal>("LocalGst"),
                        Tcs = reader.Get<decimal>("Tcs"),
                        SwachBharat = reader.Get<decimal>("SwachBharat"),
                        ExtraAmount = reader.Get<decimal>("ExtraAmount"),
                        TypeOfSale = reader.Get<string>("TypeOfSale"),
                        Prefix = reader.Get<string>("Prefix"),
                        Suffix = reader.Get<string>("Suffix"),
                        TaxOnSaleType = reader.Get<string>("TaxOnSaleType"),
                       
                        NumberStartFrom = reader.Get<int>("NumberStartFrom")
                    };
                }

                if (reader["SaleInvoiceDetailId"] != DBNull.Value)
                {
                    invoices[invoiceId].Details.Add(new SaleInvoiceDetailDto
                    {
                        SaleInvoiceDetailId = reader.Get<int>("SaleInvoiceDetailId"),
                        Barcode = reader.Get<string>("Barcode"),
                        ItemId = reader.Get<int>("ItemId"),
                        ItemName = reader.Get<string>("ItemName"),
                        Qty = reader.Get<decimal>("Qty"),
                        Rate = reader.Get<decimal>("Rate"),
                        RowTotal = reader.Get<decimal>("RowTotal"),

                        TaxTableRowSubTotal = reader.Get<decimal>("taxTableRowSubTotal"),
                        TaxableValueId = reader["TaxableValueId"] != DBNull.Value
            ? Convert.ToInt32(reader["TaxableValueId"])
            : 0,

                        Unit = reader["Unit"] != DBNull.Value
            ? Convert.ToInt32(reader["Unit"])
            : 0,

            UnitName = reader["UnitName"] != DBNull.Value
        ? reader["UnitName"].ToString()
        : "",

                        SalePurcAccountName = reader["SalePurcAccountName"] != DBNull.Value
        ? reader["SalePurcAccountName"].ToString()
        : ""

                    });
                }
            }

            return Ok(invoices.Values);
        }



        //[HttpGet("get-sale-invoice-by-id/{saleInvoiceId}")]
        //public async Task<IActionResult> GetSaleInvoiceById(int saleInvoiceId)
        //{
        //    try
        //    {
        //        var saleInvoiceIdParam = new SqlParameter("@SaleInvoiceId", SqlDbType.Int)
        //        {
        //            Value = saleInvoiceId
        //        };

        //        string jsonResult = string.Empty;

        //        DbConnection connection = context.Database.GetDbConnection();

        //        await using (connection)
        //        {
        //            await connection.OpenAsync();

        //            await using var command = connection.CreateCommand();
        //            command.CommandText = "GetSaleInvoiceById";
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.Add(saleInvoiceIdParam);

        //            await using var reader = await command.ExecuteReaderAsync();

        //            if (await reader.ReadAsync())
        //            {
        //                jsonResult = reader.GetString(0);
        //            }
        //        }

        //        if (string.IsNullOrWhiteSpace(jsonResult))
        //        {
        //            return NotFound(new
        //            {
        //                success = false,
        //                message = "Sale invoice not found"
        //            });
        //        }

        //        // 🔥 Return SQL JSON directly
        //        return Content(jsonResult, "application/json");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new
        //        {
        //            success = false,
        //            message = "Internal server error",
        //            error = ex.Message
        //        });
        //    }
        //}



        [HttpGet("get-sale-invoice-by-id/{saleInvoiceId}")]
        public async Task<IActionResult> GetSaleInvoiceById(int saleInvoiceId)
        {
            try
            {
                var saleInvoiceIdParam = new SqlParameter("@SaleInvoiceId", SqlDbType.Int)
                {
                    Value = saleInvoiceId
                };

                string jsonResult = string.Empty;

                await using DbConnection connection =
                    context.Database.GetDbConnection();

                await connection.OpenAsync();

                await using var command = connection.CreateCommand();
                command.CommandText = "GetSaleInvoiceById";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(saleInvoiceIdParam);

                await using var reader =
                    await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    // safer for large JSON
                    jsonResult += reader.GetValue(0)?.ToString();
                }

                if (string.IsNullOrWhiteSpace(jsonResult))
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Sale invoice not found"
                    });
                }

                // Return proper JSON
                return Content(jsonResult, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        success = false,
                        message = "Internal server error",
                        error = ex.Message
                    });
            }
        }

        [HttpPost("add-edit-narration")]
        public async Task<IActionResult> AddEditNarration([FromBody] tbNarration model)
        {
            try
            {
                ResponseMessageNarration? response = null;

                var query = context.Database
                    .SqlQueryRaw<ResponseMessageNarration>(
                        @"EXEC dbo.addEditNarration 
                  @Id,
                  @CompanyId,
                  @Narration",
                        new SqlParameter("@Id", model.Id),
                        new SqlParameter("@CompanyId", model.CompanyId),
                        new SqlParameter("@Narration", (object?)model.Narration ?? DBNull.Value)
                    )
                    .AsAsyncEnumerable();

                // Get first row from response
                await foreach (var item in query)
                {
                    response = item;
                    break;
                }

                return Ok(new
                {
                    success = response?.Success == 1,
                    message = response?.Message
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to save narration",
                    error = ex.Message
                });
            }
        }


        [HttpGet("get-narration-by-company/{companyId}")]
        public async Task<IActionResult> GetNarrationByCompany(int companyId)
        {
            try
            {
                var data = context.Database
                    .SqlQueryRaw<tbNarration>(
                        @"EXEC dbo.getNarrationCompanyId @CompanyId",
                        new SqlParameter("@CompanyId", companyId)
                    )
                    .ToList();

                return Ok(new
                {
                    success = true,
                    data = data
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch narration data",
                    error = ex.Message
                });
            }
        }

        [HttpGet("get-narration-by-id/{id}")]
        public async Task<IActionResult> GetNarrationById(int id)
        {
            try
            {
                var data = context.Database
                    .SqlQueryRaw<tbNarration>(
                        @"EXEC dbo.getNarrationBaseOfId @Id",
                        new SqlParameter("@Id", id)
                    )
                    .ToList();

                return Ok(new
                {
                    success = true,
                    data = data.FirstOrDefault()
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch narration",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("delete-narration/{id}")]
        public async Task<IActionResult> DeleteNarration(int id)
        {
            try
            {
                ResponseMessageNarration? response = null;

                var query = context.Database
                    .SqlQueryRaw<ResponseMessageNarration>(
                        @"EXEC dbo.deleteNarration @Id",
                        new SqlParameter("@Id", id)
                    )
                    .AsAsyncEnumerable();

                await foreach (var item in query)
                {
                    response = item;
                    break;
                }

                return Ok(new
                {
                    success = response?.Success == 1,
                    message = response?.Message
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to delete narration",
                    error = ex.Message
                });
            }
        }



        [HttpGet("get-sale-invoice-For-Pdf-id/{id}")]
        public async Task<IActionResult> GetSaleInvoiceForPdfById(int id)
        {
            try
            {
                var header = new SaleInvoiceHeaderPdf();
                var details = new List<SaleInvoiceDetailDtoPdf>();

                using var conn = context.Database.GetDbConnection();
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "dbo.GetSaleInvoiceByIdForPDF";
                cmd.CommandType = CommandType.StoredProcedure;

                var param = cmd.CreateParameter();
                param.ParameterName = "@SaleInvoiceId";
                param.Value = id;
                cmd.Parameters.Add(param);

                using var reader = await cmd.ExecuteReaderAsync();

                // -----------------------------
                // 1. Read Header
                // -----------------------------
                if (await reader.ReadAsync())
                {
                    header.SaleInvoiceId = Convert.ToInt32(reader["SaleInvoiceId"]);
                    header.InvoiceHeading = reader["InvoiceHeading"]?.ToString();
                    header.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                    header.InvoiceNo = reader["InvoiceNo"]?.ToString();
                    header.InvoiceDate = reader["InvoiceDate"] as DateTime?;
                    header.AccountId = Convert.ToInt32(reader["AccountId"]);
                    header.accountName = reader["accountName"]?.ToString();
                    header.companyPhone = reader["companyPhone"]?.ToString();
                    header.companyName = reader["companyName"]?.ToString();
                    header.cityName = reader["cityName"]?.ToString();
                    header.stateName = reader["stateName"]?.ToString();
                    header.ShipToPhone = reader["ShipToPhone"]?.ToString();
                    header.stateCode = reader["stateCode"]?.ToString();
                    header.OrderNo = reader["OrderNo"]?.ToString();
                    header.TransportName = reader["TransportName"]?.ToString();
                    header.TransportPhone = reader["TransportPhone"]?.ToString();
                    header.TransportGSTNo = reader["TransportGSTNo"]?.ToString();
                    header.TransportNameManual = reader["TransportNameManual"]?.ToString();
                    header.ShippingBillNo = reader["ShippingBillNo"]?.ToString();
                    header.GRNo = reader["GRNo"]?.ToString();
                    header.swachBharat = reader["swachBharat"] as decimal?;
                    header.tcs = reader["tcs"] as decimal?;
                    header.localGst = reader["localGst"] as decimal?;
                    header.centralGst = reader["centralGst"] as decimal?;
                    

                    header.SubTotal = reader["SubTotal"] as decimal?;
                    header.RoundAndTotal = reader["RoundAndTotal"] as decimal?;
                    header.ShipToName = reader["ShipToName"]?.ToString();
                    header.Value = reader["Value"] as decimal?;

                    header.OtherChargeName = reader["OtherChargeName"]?.ToString();
                    header.Value1 = reader["Value1"] as decimal?;

                    header.OtherCharge1Name = reader["OtherCharge1Name"]?.ToString();


                }

                // If invoice not found
                if (header.SaleInvoiceId == 0)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Invoice not found"
                    });
                }

                // -----------------------------
                // 2. Read Details
                // -----------------------------
                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        details.Add(new SaleInvoiceDetailDtoPdf
                        {
                            SaleInvoiceDetailId = Convert.ToInt32(reader["SaleInvoiceDetailId"]),
                            Barcode = reader["Barcode"]?.ToString(),
                            ItemId = Convert.ToInt32(reader["ItemId"]),
                            ItemName = reader["ItemName"]?.ToString(),
                            Remarks = reader["Remarks"]?.ToString(),
                            HSN = reader["HSN"]?.ToString(),
                            ArtNo = reader["ArtNo"]?.ToString(),
                            Size = reader["Size"]?.ToString(),
                            Color = reader["Color"]?.ToString(),
                            Pack1 = reader["Pack1"]?.ToString(),
                            Pack2 = reader["Pack2"]?.ToString(),
                            Qty = reader["Qty"] as decimal?,
                            Rate = reader["Rate"] as decimal?,
                            MRate = reader["MRate"] as decimal?,
                            DiscPer = reader["DiscPer"] as decimal?,
                            DiscAmt = reader["DiscAmt"] as decimal?,
                            TaxableValueId = reader["TaxableValueId"] as int?,
                            DetailAccountId = reader["DetailAccountId"] as int?,
                            TaxPercent = reader["TaxPercent"] as decimal?,
                            RowTotal = reader["RowTotal"] as decimal?,
                            taxTableRowSubTotal = reader["taxTableRowSubTotal"] as decimal?,
                            gstApplicabeCentralRate = reader["gstApplicabeCentralRate"] as decimal?,
                            gstApplicabeLocalRate = reader["gstApplicabeLocalRate"] as decimal?,
                            tcsApplicabeRate = reader["tcsApplicabeRate"] as decimal?,
                            swachBhartApplicableRate = reader["swachBhartApplicableRate"] as decimal?
                        });
                    }
                }

                // -----------------------------
                // Final Response
                // -----------------------------
                return Ok(new
                {
                    success = true,
                    header,
                    details
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch invoice",
                    error = ex.Message
                });
            }
        }


        [HttpPost("add-edit-sale-add-permission")]
        public async Task<IActionResult> AddEditSaleAddPermission([FromBody] salePermission model)
        {
            try
            {
                SaleAddPermissionResponse? response = null;

                var query = context.Database
                    .SqlQueryRaw<SaleAddPermissionResponse>(
                        @"EXEC dbo.InsertSaleAddPermission 
                  @CompanyId,
                  @ExcludedOrIncluded",
                        new SqlParameter("@CompanyId", model.CompanyId),
                        new SqlParameter("@ExcludedOrIncluded",
                            model.ExcludedOrIncluded ?? (object)DBNull.Value)
                    )
                    .AsAsyncEnumerable();

                await foreach (var item in query)
                {
                    response = item;
                    break;
                }

                return Ok(new
                {
                    success = true,
                    value = response?.value
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to save permission",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("deleteSaleInvoice/{id}")]
        public async Task<IActionResult> deleteSaleInvoice(int id)
        {
            try
            {
                await context.Database.ExecuteSqlRawAsync(
                    @"EXEC dbo.deleteSaleInvoice @SaleInvoiceId",
                    new SqlParameter("@SaleInvoiceId", id)
                );

                return Ok(new
                {
                    success = true,
                    message = "Sale invoice deleted successfully"
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to delete sale invoice",
                    error = ex.Message
                });
            }
        }

        //[HttpPost("add-edit-payment-receipt")]
        //public async Task<IActionResult> AddEditPaymentReceipt([FromBody] paymentReceiptVocher model)
        //{
        //    try
        //    {
        //        var detailsJson = model.Details?.Any() == true
        //            ? JsonConvert.SerializeObject(model.Details)
        //            : "[]";

        //        var parameters = new[]
        //        {
        //    new SqlParameter("@PaymentReceiptId", model.PaymentReceiptId ?? (object)DBNull.Value),
        //    new SqlParameter("@AccountName", model.AccountName),
        //    new SqlParameter("@ReceiptDate", model.ReceiptDate ?? (object)DBNull.Value),
        //    new SqlParameter("@DayName", model.DayName ?? (object)DBNull.Value),
        //    new SqlParameter("@VoucherNo", model.VoucherNo ?? (object)DBNull.Value),
        //    new SqlParameter("@NarrationForSingleAccount", model.NarrationForSingleAccount ?? (object)DBNull.Value),
        //    new SqlParameter("@CompanyId", model.CompanyId),

        //    new SqlParameter("@Details", SqlDbType.NVarChar, -1)
        //    {
        //        Value = detailsJson
        //    }
        //};

        //        var result = await context
        //            .Set<PaymentReceiptResponse>()
        //            .FromSqlRaw(
        //                @"EXEC dbo.AddEditPaymentReceipt
        //        @PaymentReceiptId,
        //        @AccountName,
        //        @ReceiptDate,
        //        @DayName,
        //        @VoucherNo,
        //        @NarrationForSingleAccount,
        //        @CompanyId,
        //        @Details",
        //                parameters)
        //            .ToListAsync();

        //        return Ok(result.FirstOrDefault());
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Success = false, Message = ex.Message });
        //    }
        //}

        [HttpPost("add-edit-payment-receipt")]
        public async Task<IActionResult> AddEditPaymentReceipt([FromBody] paymentReceiptVocher model)
        {
            try
            {
                var detailsJson = model.Details != null && model.Details.Any()
                    ? JsonConvert.SerializeObject(model.Details)
                    : "[]";

                var parameters = new[]
                {
            new SqlParameter("@PaymentReceiptId", model.PaymentReceiptId ?? (object)DBNull.Value),
            new SqlParameter("@AccountName", model.AccountName),
            new SqlParameter("@ReceiptDate", model.ReceiptDate ?? (object)DBNull.Value),
            new SqlParameter("@DayName", model.DayName ?? (object)DBNull.Value),
            new SqlParameter("@VoucherNo", model.VoucherNo ?? (object)DBNull.Value),
            new SqlParameter("@NarrationForSingleAccount", model.NarrationForSingleAccount ?? (object)DBNull.Value),
            new SqlParameter("@CompanyId", model.CompanyId),

            new SqlParameter("@Details", SqlDbType.NVarChar, -1)
            {
                Value = detailsJson
            }
        };

                var result = await context
                    .Set<PaymentReceiptResponse>()
                    .FromSqlRaw(
                        @"EXEC dbo.AddEditPaymentReceipt
                    @PaymentReceiptId,
                    @AccountName,
                    @ReceiptDate,
                    @DayName,
                    @VoucherNo,
                    @NarrationForSingleAccount,
                    @CompanyId,
                    @Details",
                        parameters)
                    .ToListAsync();

                return Ok(result.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        [HttpGet("get-payment-receipt-by-company/{companyId}")]
        public async Task<IActionResult> GetPaymentReceiptByCompany(int companyId)
        {
            try
            {
                var jsonResult = await context.Database
                    .SqlQueryRaw<string>(
                        @"EXEC dbo.getPaymentReceiptDetailsByCompanyId @CompanyId",
                        new SqlParameter("@CompanyId", companyId)
                    )
                    .ToListAsync();

                // Combine all rows into one JSON string
                var dataJson = string.Concat(jsonResult);

                var data = !string.IsNullOrEmpty(dataJson)
                    ? System.Text.Json.JsonSerializer.Deserialize<dynamic>(dataJson)
                    : new object();

                return Ok(new
                {
                    success = true,
                    data
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch payment receipt data",
                    error = ex.Message
                });
            }
        }


        [HttpGet("get-payment-receipt-by-id/{paymentReceiptId}")]
        public async Task<IActionResult> GetPaymentReceiptById(int paymentReceiptId)
        {
            try
            {
                // Execute stored procedure
                var jsonResult = await context.Database
                    .SqlQueryRaw<string>(
                        @"EXEC dbo.getPaymentReceiptDetailsById @PaymentReceiptId",
                        new SqlParameter("@PaymentReceiptId", paymentReceiptId)
                    )
                    .ToListAsync();

                // Stored procedure returns JSON string
                var dataJson = jsonResult.FirstOrDefault();

                // Convert JSON string to object
                var data = !string.IsNullOrEmpty(dataJson)
                    ? System.Text.Json.JsonSerializer.Deserialize<dynamic>(dataJson)
                    : new object();

                return Ok(new
                {
                    success = true,
                    data
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch payment receipt",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("delete-payment-receipt/{paymentReceiptId}")]
        public async Task<IActionResult> DeletePaymentReceipt(int paymentReceiptId)
        {
            try
            {
                await context.Database.ExecuteSqlRawAsync(
                    @"EXEC dbo.deletePaymentReceipt @PaymentReceiptId",
                    new SqlParameter("@PaymentReceiptId", paymentReceiptId)
                );

                return Ok(new
                {
                    success = true,
                    message = "Payment receipt deleted successfully"
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to delete payment receipt",
                    error = ex.Message
                });
            }
        }


        //[HttpGet("get-payment-receipt-by-account/{accountId}")]
        //public async Task<IActionResult> GetPaymentReceiptByAccountName(int accountId)
        //{
        //    try
        //    {
        //        // 1. Execute stored procedure. 
        //        // Note: Using string.Join because SQL FOR JSON might split long strings into multiple rows.
        //        var jsonRows = await context.Database
        //            .SqlQueryRaw<string>(
        //                @"EXEC dbo.getPaymentReceiptDetailsByAccountName @AccountId",
        //                new SqlParameter("@AccountId", accountId)
        //            )
        //            .ToListAsync();

        //        var dataJson = string.Join("", jsonRows);

        //        if (string.IsNullOrEmpty(dataJson))
        //        {
        //            return Ok(new { success = false, message = "No data found" });
        //        }

        //        // 2. Deserialize the SQL JSON string into a dynamic object
        //        var finalResult = System.Text.Json.JsonSerializer.Deserialize<dynamic>(dataJson);

        //        // 3. RETURN DIRECTLY. 
        //        // Do NOT wrap in 'new { success = true, data = finalResult }' 
        //        // because 'finalResult' already contains 'success' and 'data'.
        //        return Ok(finalResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new
        //        {
        //            success = false,
        //            message = "Internal server error",
        //            error = ex.Message
        //        });
        //    }
        //}


        [HttpGet("get-payment-receipt-by-account/{accountId}")]
        public async Task<IActionResult> GetPaymentReceiptByAccountName(int accountId,string? searchText = null,DateTime? fromDate = null,DateTime? toDate = null)
        {
            try
            {
                var jsonRows = await context.Database
                    .SqlQueryRaw<string>(
                        @"EXEC dbo.getPaymentReceiptDetailsByAccountName 
                  @AccountId, @SearchText, @FromDate, @ToDate",

                        new SqlParameter("@AccountId", accountId),
                        new SqlParameter("@SearchText", (object?)searchText ?? DBNull.Value),
                        new SqlParameter("@FromDate", (object?)fromDate ?? DBNull.Value),
                        new SqlParameter("@ToDate", (object?)toDate ?? DBNull.Value)
                    )
                    .ToListAsync();

                var dataJson = string.Join("", jsonRows);

                if (string.IsNullOrEmpty(dataJson))
                {
                    return Ok(new { success = false, message = "No data found" });
                }

                var finalResult = System.Text.Json.JsonSerializer.Deserialize<dynamic>(dataJson);

                return Ok(finalResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error",
                    error = ex.Message
                });
            }
        }





        [HttpGet("paymentRecepitFullDetailsByAccount/{accountId}")]
        public async Task<IActionResult> paymentRecepitFullDetailsByAccount (int accountId, string? searchText = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var jsonRows = await context.Database
                    .SqlQueryRaw<string>(
                        @"EXEC dbo.paymentRecepitFullDetails 
                  @AccountId, @SearchText, @FromDate, @ToDate",

                        new SqlParameter("@AccountId", accountId),
                        new SqlParameter("@SearchText", (object?)searchText ?? DBNull.Value),
                        new SqlParameter("@FromDate", (object?)fromDate ?? DBNull.Value),
                        new SqlParameter("@ToDate", (object?)toDate ?? DBNull.Value)
                    )
                    .ToListAsync();

                var dataJson = string.Join("", jsonRows);

                if (string.IsNullOrEmpty(dataJson))
                {
                    return Ok(new { success = false, message = "No data found" });
                }

                var finalResult = System.Text.Json.JsonSerializer.Deserialize<dynamic>(dataJson);

                return Ok(finalResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error",
                    error = ex.Message
                });
            }
        }





        [HttpPost("add-edit-purchase")]
        public async Task<IActionResult> AddEditPurchaseInvoice(
            [FromBody] PurchaseInvoiceRequest model)
        {
            try
            {
                // SAFETY: ensure JSON array
                var detailsJson = model.PurchaseInvoiceDetails?.Any() == true
                    ? JsonConvert.SerializeObject(model.PurchaseInvoiceDetails)
                    : "[]";

                var parameters = new[]
                {
            new SqlParameter(
                "@PurchaseInvoiceId",
                model.PurchaseInvoiceId ?? (object)DBNull.Value),

            new SqlParameter(
                "@InvoiceHeading",
                model.InvoiceHeading ?? (object)DBNull.Value),

            new SqlParameter(
                "@InvoiceHeadingInt",
                model.InvoiceHeadingInt ?? (object)DBNull.Value),

            new SqlParameter(
                "@CompanyId",
                model.CompanyId),

            new SqlParameter(
                "@InvoiceNo",
                model.InvoiceNo ?? (object)DBNull.Value),

            new SqlParameter(
                "@InvoiceDate",
                model.InvoiceDate),

            new SqlParameter(
                "@ClaimDate",
                model.ClaimDate ?? (object)DBNull.Value),

            new SqlParameter(
                "@AccountId",
                model.AccountId),

            new SqlParameter(
                "@ShipTo",
                model.ShipTo ?? (object)DBNull.Value),

            new SqlParameter(
                "@Transport",
                model.Transport ?? (object)DBNull.Value),

            new SqlParameter(
                "@TransportNameManual",
                model.TransportNameManual ?? (object)DBNull.Value),

            new SqlParameter(
                "@ShippingBillNo",
                model.ShippingBillNo ?? (object)DBNull.Value),

            new SqlParameter(
                "@GRNo",
                model.GRNo ?? (object)DBNull.Value),

            new SqlParameter(
                "@OrderNo",
                model.OrderNo ?? (object)DBNull.Value),

            new SqlParameter(
                "@Vehicle",
                model.Vehicle ?? (object)DBNull.Value),

            new SqlParameter(
                "@FormNo",
                model.FormNo ?? (object)DBNull.Value),

            new SqlParameter(
                "@Weight",
                model.Weight ?? (object)DBNull.Value),

            new SqlParameter(
                "@CreditDays",
                model.CreditDays ?? (object)DBNull.Value),

            new SqlParameter(
                "@PackingNo",
                model.PackingNo ?? (object)DBNull.Value),

            new SqlParameter(
                "@DocuThru",
                model.DocuThru ?? (object)DBNull.Value),

            new SqlParameter(
                "@Station",
                model.Station ?? (object)DBNull.Value),

            new SqlParameter(
                "@RGPNo",
                model.RGPNo ?? (object)DBNull.Value),

            new SqlParameter(
                "@Dated",
                model.Dated ?? (object)DBNull.Value),

            new SqlParameter(
                "@Freight",
                model.Freight ?? (object)DBNull.Value),

            new SqlParameter(
                "@Packages",
                model.Packages ?? (object)DBNull.Value),

            new SqlParameter(
                "@PvtMark",
                model.PvtMark ?? (object)DBNull.Value),

            new SqlParameter(
                "@DueDate",
                model.DueDate ?? (object)DBNull.Value),

            new SqlParameter(
                "@EcomGSTIN",
                model.EcomGSTIN ?? (object)DBNull.Value),

            new SqlParameter(
                "@EwayNo",
                model.EwayNo ?? (object)DBNull.Value),

            new SqlParameter(
                "@ShBNo",
                model.ShBNo ?? (object)DBNull.Value),

            new SqlParameter(
                "@ShipDate",
                model.ShipDate ?? (object)DBNull.Value),

            new SqlParameter(
                "@ShipPartNo",
                model.ShipPartNo ?? (object)DBNull.Value),

            new SqlParameter(
                "@PortLoading",
                model.PortLoading ?? (object)DBNull.Value),

            new SqlParameter(
                "@PortDischarge",
                model.PortDischarge ?? (object)DBNull.Value),

            new SqlParameter(
                "@FinalDestination",
                model.FinalDestination ?? (object)DBNull.Value),

            new SqlParameter(
                "@EntrBy",
                model.EntrBy ?? (object)DBNull.Value),

            new SqlParameter(
                "@SubTotal",
                model.SubTotal ?? (object)DBNull.Value),

            new SqlParameter(
                "@RoundAndTotal",
                model.RoundAndTotal ?? (object)DBNull.Value),

            new SqlParameter(
                "@TaxablePurchase",
                model.TaxablePurchase ?? (object)DBNull.Value),

            new SqlParameter(
                "@CentralGst",
                model.CentralGst ?? (object)DBNull.Value),

            new SqlParameter(
                "@LocalGst",
                model.LocalGst ?? (object)DBNull.Value),

            new SqlParameter(
                "@Tcs",
                model.Tcs ?? (object)DBNull.Value),

            new SqlParameter(
                "@SwachBharat",
                model.SwachBharat ?? (object)DBNull.Value),

            new SqlParameter(
                "@Value",
                model.Value ?? (object)DBNull.Value),

            new SqlParameter(
                "@OtherCharge",
                model.OtherCharge),

            new SqlParameter(
                "@Value1",
                model.Value1 ?? (object)DBNull.Value),

            new SqlParameter(
                "@OtherCharge1",
                model.OtherCharge1),

            new SqlParameter(
                "@ExtraAmount",
                model.ExtraAmount),

            // Purchase invoice detail JSON
            new SqlParameter(
                "@PurchaseInvoiceDetails",
                SqlDbType.NVarChar, -1)
            {
                Value = detailsJson
            }
        };

                var result = await context
                    .Set<PurchaseInvoiceApiResponse>()
                    .FromSqlRaw(
                        @"EXEC dbo.sp_AddOrUpdate_PurchaseInvoice
                    @PurchaseInvoiceId,
                    @InvoiceHeading,
                    @InvoiceHeadingInt,
                    @CompanyId,
                    @InvoiceNo,
                    @InvoiceDate,
                    @ClaimDate,
                    @AccountId,
                    @ShipTo,
                    @Transport,
                    @TransportNameManual,
                    @ShippingBillNo,
                    @GRNo,
                    @OrderNo,
                    @Vehicle,
                    @FormNo,
                    @Weight,
                    @CreditDays,
                    @PackingNo,
                    @DocuThru,
                    @Station,
                    @RGPNo,
                    @Dated,
                    @Freight,
                    @Packages,
                    @PvtMark,
                    @DueDate,
                    @EcomGSTIN,
                    @EwayNo,
                    @ShBNo,
                    @ShipDate,
                    @ShipPartNo,
                    @PortLoading,
                    @PortDischarge,
                    @FinalDestination,
                    @EntrBy,
                    @SubTotal,
                    @RoundAndTotal,
                    @TaxablePurchase,
                    @CentralGst,
                    @LocalGst,
                    @Tcs,
                    @SwachBharat,
                    @Value,
                    @OtherCharge,
                    @Value1,
                    @OtherCharge1,
                    @ExtraAmount,
                    @PurchaseInvoiceDetails",
                        parameters)
                    .ToListAsync();

                return Ok(result.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("purchase-invoices/{companyId}")]
        public async Task<IActionResult> GetPurchaseInvoicesByCompanyId(int companyId)
        {
            var invoices = new Dictionary<int, PurchaseInvoiceDto>();

            using var conn = context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();

            cmd.CommandText = "GetPurchaseInvoicesByCompanyId";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(
                new SqlParameter("@CompanyId", companyId)
            );

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                int invoiceId = reader.Get<int>("PurchaseInvoiceId");

                // ================================
                // Purchase Invoice Header
                // ================================
                if (!invoices.ContainsKey(invoiceId))
                {
                    invoices[invoiceId] = new PurchaseInvoiceDto
                    {
                        PurchaseInvoiceId = invoiceId,

                        InvoiceHeading = reader.Get<string>("InvoiceHeading"),

                        InvoiceHeadingInt = reader.Get<int>("InvoiceHeadingInt"),

                        CompanyId = reader.Get<int>("CompanyId"),

                        InvoiceNo = reader.Get<string>("InvoiceNo"),

                        InvoiceDate = reader.Get<DateTime>("InvoiceDate"),

                        ClaimDate = reader.Get<DateTime>("ClaimDate"),

                        AccountId = reader.Get<int>("AccountId"),

                        AccountName = reader.Get<string>("accountName"),

                        ShipTo = reader.Get<int>("ShipTo"),

                        ShipToName = reader.Get<string>("ShipToName"),

                        TransportNameManual =
                            reader.Get<string>("TransportNameManual"),

                        PvtMark =
                            reader.Get<string>("PvtMark"),

                        DueDate =
                            reader.Get<string>("DueDate"),

                        SubTotal =
                            reader.Get<decimal>("SubTotal"),

                        RoundAndTotal =
                            reader.Get<decimal>("RoundAndTotal"),

                        TaxablePurchase =
                            reader.Get<decimal>("TaxablePurchase"),

                        CentralGst =
                            reader.Get<decimal>("CentralGst"),

                        LocalGst =
                            reader.Get<decimal>("LocalGst"),

                        Tcs =
                            reader.Get<decimal>("Tcs"),

                        SwachBharat =
                            reader.Get<decimal>("SwachBharat"),

                        ExtraAmount =
                            reader.Get<decimal>("ExtraAmount"),

                        TypeOfPurchase =
                            reader.Get<string>("TypeOfPurchase"),

                        Prefix =
                            reader.Get<string>("Prefix"),

                        Suffix =
                            reader.Get<string>("Suffix"),

                        TaxOnPurchaseType =
                            reader.Get<string>("TaxOnPurchaseType"),

                        NumberStartFrom =
                            reader.Get<int>("NumberStartFrom")
                    };
                }

                // ================================
                // Purchase Invoice Detail
                // ================================
                if (reader["PurchaseInvoiceDetailId"] != DBNull.Value)
                {
                    invoices[invoiceId].Details.Add(
                        new PurchaseInvoiceDetailDto
                        {
                            PurchaseInvoiceDetailId =
                                reader.Get<int>("PurchaseInvoiceDetailId"),

                            Barcode =
                                reader.Get<string>("Barcode"),

                            ItemId =
                                reader.Get<int>("ItemId"),

                            ItemName =
                                reader.Get<string>("ItemName"),

                            Qty =
                                reader.Get<decimal>("Qty"),

                            Rate =
                                reader.Get<decimal>("Rate"),

                            RowTotal =
                                reader.Get<decimal>("RowTotal"),

                            TaxTableRowSubTotal =
                                reader.Get<decimal>("TaxTableRowSubTotal"),

                            TaxableValueId =
                                reader["TaxableValueId"] != DBNull.Value
                                    ? Convert.ToInt32(
                                        reader["TaxableValueId"])
                                    : 0,

                            Unit =
                                reader["Unit"] != DBNull.Value
                                    ? Convert.ToInt32(
                                        reader["Unit"])
                                    : 0,

                            UnitName =
                                reader["UnitName"] != DBNull.Value
                                    ? reader["UnitName"].ToString()
                                    : "",

                            SalePurcAccountName =
                                reader["SalePurcAccountName"] != DBNull.Value
                                    ? reader["SalePurcAccountName"].ToString()
                                    : ""
                        }
                    );
                }
            }

            return Ok(invoices.Values);
        }



        [HttpGet("get-Purchase-invoice-by-id/{PurchaseInvoiceId}")]
        public async Task<IActionResult> GetPurchaseInvoiceById(int PurchaseInvoiceId)
        {
            try
            {
                var purchaseInvoiceIdParam = new SqlParameter("@PurchaseInvoiceId", SqlDbType.Int)
                {
                    Value = PurchaseInvoiceId
                };

                string jsonResult = string.Empty;

                await using DbConnection connection =
                    context.Database.GetDbConnection();

                await connection.OpenAsync();

                await using var command = connection.CreateCommand();
                command.CommandText = "GetPurchaseInvoiceById";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(purchaseInvoiceIdParam);

                await using var reader =
                    await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    // safer for large JSON
                    jsonResult += reader.GetValue(0)?.ToString();
                }

                if (string.IsNullOrWhiteSpace(jsonResult))
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Purchase invoice not found"
                    });
                }

                // Return proper JSON
                return Content(jsonResult, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        success = false,
                        message = "Internal server error",
                        error = ex.Message
                    });
            }
        }

        [HttpDelete("deletePurchaseInvoice/{id}")]
        public async Task<IActionResult> deletePurchaseInvoice(int id)
        {
            try
            {
                await context.Database.ExecuteSqlRawAsync(
                    @"EXEC dbo.DeletePurchaseInvoice @PurchaseInvoiceId",
                    new SqlParameter("@PurchaseInvoiceId", id)
                );

                return Ok(new
                {
                    success = true,
                    message = "Purchase invoice deleted successfully"
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to delete Purchase invoice",
                    error = ex.Message
                });
            }
        }


        [HttpGet("get-purchase-invoice-For-Pdf-id/{id}")]
        public async Task<IActionResult> GetPurchaseInvoiceForPdfById(int id)
        {
            try
            {
                var header = new PurchaseInvoiceHeaderPdf();
                var details = new List<PurchaseInvoiceDetailDtoPdf>();

                using var conn = context.Database.GetDbConnection();
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "dbo.GetPurchaseInvoiceByIdForPDF";
                cmd.CommandType = CommandType.StoredProcedure;

                var param = cmd.CreateParameter();
                param.ParameterName = "@PurchaseInvoiceId";
                param.Value = id;
                cmd.Parameters.Add(param);

                using var reader = await cmd.ExecuteReaderAsync();

                // -----------------------------
                // 1. Read Header
                // -----------------------------
                if (await reader.ReadAsync())
                {
                    header.PurchaseInvoiceId = Convert.ToInt32(reader["PurchaseInvoiceId"]);
                    header.InvoiceHeading = reader["InvoiceHeading"]?.ToString();
                    header.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                    header.InvoiceNo = reader["InvoiceNo"]?.ToString();
                    header.InvoiceDate = reader["InvoiceDate"] as DateTime?;
                    header.AccountId = Convert.ToInt32(reader["AccountId"]);
                    header.accountName = reader["accountName"]?.ToString();
                    header.companyPhone = reader["companyPhone"]?.ToString();
                    header.companyName = reader["companyName"]?.ToString();
                    header.cityName = reader["cityName"]?.ToString();
                    header.stateName = reader["stateName"]?.ToString();
                    header.ShipToPhone = reader["ShipToPhone"]?.ToString();
                    header.stateCode = reader["stateCode"]?.ToString();
                    header.OrderNo = reader["OrderNo"]?.ToString();
                    header.TransportName = reader["TransportName"]?.ToString();
                    header.TransportPhone = reader["TransportPhone"]?.ToString();
                    header.TransportGSTNo = reader["TransportGSTNo"]?.ToString();
                    header.TransportNameManual = reader["TransportNameManual"]?.ToString();
                    header.ShippingBillNo = reader["ShippingBillNo"]?.ToString();
                    header.GRNo = reader["GRNo"]?.ToString();
                    header.swachBharat = reader["swachBharat"] as decimal?;
                    header.tcs = reader["tcs"] as decimal?;
                    header.localGst = reader["localGst"] as decimal?;
                    header.centralGst = reader["centralGst"] as decimal?;


                    header.SubTotal = reader["SubTotal"] as decimal?;
                    header.RoundAndTotal = reader["RoundAndTotal"] as decimal?;
                    header.ShipToName = reader["ShipToName"]?.ToString();
                    header.Value = reader["Value"] as decimal?;

                    header.OtherChargeName = reader["OtherChargeName"]?.ToString();
                    header.Value1 = reader["Value1"] as decimal?;

                    header.OtherCharge1Name = reader["OtherCharge1Name"]?.ToString();


                }

                // If invoice not found
                if (header.PurchaseInvoiceId == 0)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Invoice not found"
                    });
                }

                // -----------------------------
                // 2. Read Details
                // -----------------------------
                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        details.Add(new PurchaseInvoiceDetailDtoPdf
                        {
                            PurchaseInvoiceDetailId = Convert.ToInt32(reader["PurchaseInvoiceDetailId"]),
                            Barcode = reader["Barcode"]?.ToString(),
                            ItemId = Convert.ToInt32(reader["ItemId"]),
                            ItemName = reader["ItemName"]?.ToString(),
                            Remarks = reader["Remarks"]?.ToString(),
                            HSN = reader["HSN"]?.ToString(),
                            ArtNo = reader["ArtNo"]?.ToString(),
                            Size = reader["Size"]?.ToString(),
                            Color = reader["Color"]?.ToString(),
                            Pack1 = reader["Pack1"]?.ToString(),
                            Pack2 = reader["Pack2"]?.ToString(),
                            Qty = reader["Qty"] as decimal?,
                            Rate = reader["Rate"] as decimal?,
                            MRate = reader["MRate"] as decimal?,
                            DiscPer = reader["DiscPer"] as decimal?,
                            DiscAmt = reader["DiscAmt"] as decimal?,
                            TaxableValueId = reader["TaxableValueId"] as int?,
                            DetailAccountId = reader["DetailAccountId"] as int?,
                            TaxPercent = reader["TaxPercent"] as decimal?,
                            RowTotal = reader["RowTotal"] as decimal?,
                            TaxTableRowSubTotal = reader["TaxTableRowSubTotal"] as decimal?,
                            gstApplicabeCentralRate = reader["gstApplicabeCentralRate"] as decimal?,
                            gstApplicabeLocalRate = reader["gstApplicabeLocalRate"] as decimal?,
                            tcsApplicabeRate = reader["tcsApplicabeRate"] as decimal?,
                            swachBhartApplicableRate = reader["swachBhartApplicableRate"] as decimal?
                        });
                    }
                }

                // -----------------------------
                // Final Response
                // -----------------------------
                return Ok(new
                {
                    success = true,
                    header,
                    details
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch invoice",
                    error = ex.Message
                });
            }
        }


        [HttpGet("get-account-ledger")]
        public async Task<IActionResult> GetAccountLedger(
      int companyId,
      int accountId,
      DateTime? fromDate = null,
      DateTime? toDate = null)
        {
            try
            {
                var ledger = new List<AccountLedgerDto>();

                using var conn = context.Database.GetDbConnection();

                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();

                cmd.CommandText = "dbo.sp_GetAccountLedger";
                cmd.CommandType = CommandType.StoredProcedure;

                // CompanyId
                var companyParam = cmd.CreateParameter();
                companyParam.ParameterName = "@CompanyId";
                companyParam.Value = companyId;
                cmd.Parameters.Add(companyParam);

                // AccountId
                var accountParam = cmd.CreateParameter();
                accountParam.ParameterName = "@AccountId";
                accountParam.Value = accountId;
                cmd.Parameters.Add(accountParam);

                // FromDate
                var fromDateParam = cmd.CreateParameter();
                fromDateParam.ParameterName = "@FromDate";
                fromDateParam.Value =
                    fromDate.HasValue
                        ? fromDate.Value.Date
                        : DBNull.Value;

                cmd.Parameters.Add(fromDateParam);

                // ToDate
                var toDateParam = cmd.CreateParameter();
                toDateParam.ParameterName = "@ToDate";
                toDateParam.Value =
                    toDate.HasValue
                        ? toDate.Value.Date
                        : DBNull.Value;

                cmd.Parameters.Add(toDateParam);

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    ledger.Add(new AccountLedgerDto
                    {
                        Date = reader["Date"] == DBNull.Value
                            ? null
                            : Convert.ToDateTime(reader["Date"]),

                        Particulars = reader["Particulars"] == DBNull.Value
                            ? null
                            : reader["Particulars"].ToString(),

                        VoucherType = reader["Vch Type"] == DBNull.Value
                            ? null
                            : reader["Vch Type"].ToString(),

                        VoucherNo = reader["Vch No"] == DBNull.Value
                            ? null
                            : reader["Vch No"].ToString(),

                        Qty = reader["Qty"] == DBNull.Value
                            ? null
                            : Convert.ToDecimal(reader["Qty"]),

                        Items = reader["Items"] == DBNull.Value
                            ? null
                            : reader["Items"].ToString(),

                        Debit = reader["Debit"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(reader["Debit"]),

                        Credit = reader["Credit"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(reader["Credit"]),

                        Balance = reader["Balance"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(reader["Balance"]),

                        BalanceType = reader["Balance Type"] == DBNull.Value
                            ? null
                            : reader["Balance Type"].ToString()
                    });
                }

                return Ok(new
                {
                    success = true,
                    companyId = companyId,
                    accountId = accountId,
                    fromDate = fromDate,
                    toDate = toDate,
                    count = ledger.Count,
                    data = ledger
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to fetch account ledger",
                    error = ex.Message
                });
            }
        }

    }




}

        

