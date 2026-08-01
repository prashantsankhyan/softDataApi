using System;
using System.Collections.Generic;

namespace softDataApi.Models;

public partial class TaxTableMaster
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public int? SalePurcAccountName { get; set; }

    public string? SelectType { get; set; }

    public string? UnderVatReturn { get; set; }

    public bool? ExciseApplicabe { get; set; }

    public bool? GstApplicabeCentral { get; set; }

    public int? GstApplicabeCentralName { get; set; }

    public decimal? GstApplicabeCentralRate { get; set; }

    public string? GstApplicabeCentralCalculateOn { get; set; }

    public bool? GstApplicabeLocal { get; set; }

    public int? GstApplicabeLocalName { get; set; }

    public decimal? GstApplicabeLocalRate { get; set; }

    public string? GstApplicabeLocalCalculateOn { get; set; }

    public bool? TcsApplicabe { get; set; }

    public int? TcsApplicabeName { get; set; }

    public decimal? TcsApplicabeRate { get; set; }

    public string? TcsApplicabeCalculateOn { get; set; }

    public bool? SwachBhartApplicable { get; set; }

    public int? SwachBhartApplicableName { get; set; }

    public decimal? SwachBhartApplicableRate { get; set; }

    public string? SwachBhartApplicableCalculateOn { get; set; }

    public decimal? Totalax { get; set; }

    public decimal? SubTotalTax { get; set; }
}



public class TaxTableDetailsDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }

    public int? SalePurcAccountId { get; set; }
    public string? SalePurcAccountName { get; set; }

    public string? SelectType { get; set; }
    public string? UnderVatReturn { get; set; }

    public bool? ExciseApplicabe { get; set; }

    public bool? GstApplicabeCentral { get; set; }
    public int? GstApplicabeCentralId { get; set; }
    public string? GstApplicabeCentralName { get; set; }
    public decimal? GstApplicabeCentralRate { get; set; }
    public string? GstApplicabeCentralCalculateOn { get; set; }

    public bool? GstApplicabeLocal { get; set; }
    public int? GstApplicabeLocalId { get; set; }
    public string? GstApplicabeLocalName { get; set; }
    public decimal? GstApplicabeLocalRate { get; set; }
    public string? GstApplicabeLocalCalculateOn { get; set; }

    public bool? TcsApplicabe { get; set; }
    public int? TcsApplicabeId { get; set; }
    public string? TcsApplicabeName { get; set; }
    public decimal? TcsApplicabeRate { get; set; }
    public string? TcsApplicabeCalculateOn { get; set; }

    public bool? SwachBhartApplicable { get; set; }
    public int? SwachBhartApplicableId { get; set; }
    public string? SwachBhartApplicableName { get; set; }
    public decimal? SwachBhartApplicableRate { get; set; }
    public string? SwachBhartApplicableCalculateOn { get; set; }

    public decimal? Totalax { get; set; }
    public decimal? SubTotalTax { get; set; }
}
