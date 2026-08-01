using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace softDataApi.Models;

public partial class TbAccountMaster
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public int? GroupId { get; set; }

    public string? StateUser { get; set; }

    public int? TransportId { get; set; }

    public string? AccountName { get; set; }

    public string? Address { get; set; }

    public string? ZipCode { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Gst { get; set; }

    public string? Pan { get; set; }

    public string? AdharNo { get; set; }

    public string? ContectName { get; set; }

    public string? ContectNo { get; set; }

    public string? BankName { get; set; }

    public string? IfscCode { get; set; }

    public int? CreditDay { get; set; }

    public string? RegdType { get; set; }

    public int? RegistrationTypeId { get; set; }

    public bool? IsTcsCompulsary { get; set; }

    public bool? TdsApplicabe { get; set; }

    public bool? IsItTransport { get; set; }

    public string? TransportMode { get; set; }

    public decimal? TransportRate { get; set; }

    public decimal? TcsLimit { get; set; }

    public int? CityId { get; set; }

    public string? TanNo { get; set; }

    public decimal? OpBalance { get; set; }

    public string? OpeningBalanceType { get; set; }

    public decimal? ClsBalance { get; set; }

    public string? ClosingBalanceType { get; set; }

    public string? AgentAndAreaName { get; set; }

    public string? OpeningBalance { get; set; }

    public string? ClosingBalance { get; set; }

    public int? State { get; set; }

    public bool? GstVatReturn { get; set; }

    public bool? MaintBillWise { get; set; }

    public string? EComNo { get; set; }

    public string? ExpHsnCode { get; set; }

    public string? CinNo { get; set; }

    public string? IeCodeNo { get; set; }

    public string? CreditLimit { get; set; }

    public string? InttRate { get; set; }

    public string? BasicLimit { get; set; }

    public string? TaxFormName { get; set; }

    public string? TradeType { get; set; }
    public string? Comments { get; set; }
    
    public int? AgentId { get; set; }

    public int GstVat { get; set; }
}
public class AccountListDto
{
    [Column("_id")]        // ✅ maps SQL _id → C# Id
    public int Id { get; set; }
    public int CompanyId { get; set; }

    public int? GroupId { get; set; }
    public string? GroupName { get; set; }

    public string? GroupCategoryName { get; set; }  // 🔥 new property

    public string? StateUser { get; set; }

    public int? TransportId { get; set; }
    public string? TransportName { get; set; }
    public string? TransportPhone { get; set; }
    public string? TransportGSTNo { get; set; }

    public string? AccountName { get; set; }
    public string? Address { get; set; }
    public string? ZipCode { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }

    public string? Gst { get; set; }
    public string? Pan { get; set; }
    public string? AdharNo { get; set; }

    public string? ContectName { get; set; }
    public string? ContectNo { get; set; }

    public string? BankName { get; set; }
    public string? IfscCode { get; set; }

    public int? CreditDay { get; set; }
    public string? RegdType { get; set; }
    public int? RegistrationTypeId { get; set; }
    public string? RegistrationType { get; set; }

    public bool? IsTcsCompulsary { get; set; }
    public bool? TdsApplicabe { get; set; }
    public bool? IsItTransport { get; set; }

    public string? TransportMode { get; set; }
    public decimal? TransportRate { get; set; }
    public decimal? TcsLimit { get; set; }

    public int? CityId { get; set; }
    public string? TanNo { get; set; }

    public string? CityName { get; set; }


    public decimal? OpBalance { get; set; }
    public string? OpeningBalanceType { get; set; }
    public decimal? ClsBalance { get; set; }
    public string? ClosingBalanceType { get; set; }

    public string? AgentAndAreaName { get; set; }
    public string? OpeningBalance { get; set; }
    public string? ClosingBalance { get; set; }

    //public int? State { get; set; }
    public string? StateName { get; set; }
    public string? StateCode { get; set; }

    public bool? GstVatReturn { get; set; }
    public bool? MaintBillWise { get; set; }

    public string? EComNo { get; set; }
    public string? ExpHsnCode { get; set; }
    public string? CinNo { get; set; }
    public string? IeCodeNo { get; set; }

    public string? CreditLimit { get; set; }
    public string? InttRate { get; set; }
    public string? BasicLimit { get; set; }
    public string? TaxFormName { get; set; }
    public string? TradeType { get; set; }

    // Agent
    public int? AgentId { get; set; }
    public string? AgentName { get; set; }
    public string? AgentAddress { get; set; }
    public string? AgentCity { get; set; }
    public string? AgentPinCode { get; set; }
    public string? AgentPhone { get; set; }
    public string? AgentPan { get; set; }
    public string? Comments { get; set; }

    // GST VAT
    public int GstVatId { get; set; }
    public string? GstVatClassName { get; set; }
    public decimal GstVatRate { get; set; }
    public string? GstVatType { get; set; }
    public int GstVatHeadingId { get; set; }
    public string? GstVatHeadingName { get; set; }
}

public class AccountByIdDto
{
    [Column("_id")]
    public int Id { get; set; }

    public int CompanyId { get; set; }
    public int? GroupId { get; set; }
    public string? StateUser { get; set; }

    public int? TransportId { get; set; }
    public string? AccountName { get; set; }
    public string? Address { get; set; }
    public string? ZipCode { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }

    public string? Gst { get; set; }
    public string? Pan { get; set; }
    public string? AdharNo { get; set; }

    public string? ContectName { get; set; }
    public string? ContectNo { get; set; }

    public string? BankName { get; set; }
    public string? IfscCode { get; set; }

    public int? CreditDay { get; set; }
    public string? RegdType { get; set; }

    public int? RegistrationTypeId { get; set; }

    public bool? IsTcsCompulsary { get; set; }
    public bool? TdsApplicabe { get; set; }
    public bool? IsItTransport { get; set; }

    public string? TransportMode { get; set; }
    public decimal? TransportRate { get; set; }
    public decimal? TcsLimit { get; set; }

    public int? CityId { get; set; }
    public string? TanNo { get; set; }

    public decimal? OpBalance { get; set; }
    public string? OpeningBalanceType { get; set; }
    public decimal? ClsBalance { get; set; }
    public string? ClosingBalanceType { get; set; }

    public string? AgentAndAreaName { get; set; }
    public string? OpeningBalance { get; set; }
    public string? ClosingBalance { get; set; }

    public int? State { get; set; }
    public bool? GstVatReturn { get; set; }
    public bool? MaintBillWise { get; set; }

    public string? EComNo { get; set; }
    public string? ExpHsnCode { get; set; }
    public string? CinNo { get; set; }
    public string? IeCodeNo { get; set; }

    public string? CreditLimit { get; set; }
    public string? InttRate { get; set; }
    public string? BasicLimit { get; set; }
    public string? TaxFormName { get; set; }
    public string? TradeType { get; set; }
    public string? Comments { get; set; }
    public int? AgentId { get; set; }
    public int? GstVat { get; set; }
}
public class DbMessageDto
{
    public string? Message { get; set; }
}

public class AccountDuplicateCheckDto
{
    public bool? existsData { get; set; }
    public string? message { get; set; }
}