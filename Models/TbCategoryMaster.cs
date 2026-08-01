using System;
using System.Collections.Generic;

namespace softDataApi.Models;

public partial class TbCategoryMaster
{
    public int CategoryId { get; set; }

    public int CompanyId { get; set; }

    public string? CategoryName { get; set; }

    public string? CategoryType { get; set; }
}
