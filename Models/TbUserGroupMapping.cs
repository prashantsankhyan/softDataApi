using System;
using System.Collections.Generic;

namespace softDataApi.Models;

public partial class TbUserGroupMapping
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int GroupId { get; set; }

    public int CompanyId { get; set; }
}
