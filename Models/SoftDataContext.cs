using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace softDataApi.Models;

public partial class SoftDataContext : DbContext
{
    public SoftDataContext()
    {
    }

    public SoftDataContext(DbContextOptions<SoftDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<GstVatAccout> GstVatAccouts { get; set; }
    public DbSet<GstVatAccountListDto> GstVatAccountList { get; set; }

    public virtual DbSet<SaleHeading> SaleHeadings { get; set; }
    public DbSet<SaleHeadingDto> SaleHeadingDto { get; set; }

    public DbSet<purchaseById> purchaseById { get; set; }

    
    public DbSet<purchaseDto> purchaseDto { get; set; }

    public DbSet<SaleHeadingById> SaleHeadingById { get; set; }

    public virtual DbSet<SaleInvoice> SaleInvoices { get; set; }

    public DbSet<NextInvoiceNoDto> NextInvoiceNo { get; set; }

    public DbSet<NextInvoiceHeadingNoDto> NextInvoiceHeadingNoDtos { get; set; }
    public virtual DbSet<SaleInvoiceDetail> SaleInvoiceDetails { get; set; }

    public virtual DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }

    public virtual DbSet<PurchaseInvoiceDetail> PurchaseInvoiceDetails { get; set; }

    public virtual DbSet<ScreenManagement> ScreenManagements { get; set; }

    public virtual DbSet<SubUserDetail> SubUserDetails { get; set; }
    public DbSet<GroupDto> Groups { get; set; }
    public DbSet<GroupMasterZero> GroupMasterZero { get; set; }
    public virtual DbSet<TaxTableMaster> TaxTableMasters { get; set; }

    public virtual DbSet<TbAccountMaster> TbAccountMasters { get; set; }

    public DbSet<AccountListDto> AccountListDto { get; set; }
    public DbSet<AccountByIdDto> AccountByIdDto { get; set; }
    public DbSet<DbMessageDto> DbMessageDto { get; set; }
    public DbSet<TaxTableDetailsDto> TaxTableDetailsDto { get; set; }
    public virtual DbSet<TbAgentMaster> TbAgentMasters { get; set; }
    public DbSet<AddEditAgentResult> AddEditAgentResults { get; set; } = null!;

    public virtual DbSet<TbAppearInMaster> TbAppearInMasters { get; set; }

    public virtual DbSet<TbCategoryMaster> TbCategoryMasters { get; set; }

    public virtual DbSet<TbCityMaster> TbCityMasters { get; set; }

    public DbSet<CityDetailsDto> CityDetailsDto { get; set; }   // ✅ ADD THIS
    public DbSet<CityDetails> CityDetails { get; set; }   // ✅ ADD THIS
    public DbSet<ClientDetailsDto> ClientDetailsDto { get; set; }
    public DbSet<ClientDetailsResponse> ClientDetailsResponse { get; set; }

    public virtual DbSet<TbClientLoginMaster> TbClientLoginMasters { get; set; }
    public DbSet<AddEditClientRequest> AddEditClientRequest { get; set; }

    public virtual DbSet<TbCompanyDetailsMaster> TbCompanyDetailsMasters { get; set; }
    public DbSet<CompanyDetailsDto> CompanyDetailsDto { get; set; }

    public DbSet<CompanyIdResponse> CompanyIdResponses { get; set; }


    public DbSet<SubUserLoginDto> SubUserLoginDtos { get; set; }

    public virtual DbSet<TbGroupCategoryMaster> TbGroupCategoryMasters { get; set; }

    public virtual DbSet<TbGroupMaster> TbGroupMasters { get; set; }

    public virtual DbSet<TbHeadingNameMaster> TbHeadingNameMasters { get; set; }
    public DbSet<CommonSpResponse> CommonSpResponse { get; set; } = null!;

    public virtual DbSet<TbItemGroupMaster> TbItemGroupMasters { get; set; }
    public virtual DbSet<ItemGroupDTOComanyId> ItemGroupDTOComanyId { get; set; }

    
    public DbSet<ItemMasterListDto> ItemMasterList { get; set; }
    public DbSet<ItemGroupMasterById> ItemGroupMasterById { get; set; }

    public DbSet<ApiResponse> ApiResponses { get; set; }

    public DbSet<PurchaseInvoiceApiResponse> PurchaseInvoiceApiResponses { get; set; }
    public virtual DbSet<TbItemMaster> TbItemMasters { get; set; }

    public virtual DbSet<TbStateMaster> TbStateMasters { get; set; }

    public virtual DbSet<AddTbState> AddTbState { get; set; }

    public virtual DbSet<TbTransportMaster> TbTransportMasters { get; set; }

    public virtual DbSet<TbUnitMaster> TbUnitMasters { get; set; }

    public virtual DbSet<TbUserGroupMapping> TbUserGroupMappings { get; set; }

    public virtual DbSet<PaymentReceiptResponse> PaymentReceiptResponse { get; set; }
    public DbSet<AccountDuplicateCheckDto> AccountDuplicateCheckDto { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dharmesh");

        modelBuilder.Entity<GstVatAccout>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__gstVatAc__3213E83F5FE05D13");

            entity.ToTable("gstVatAccout", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClassName)
                .HasMaxLength(200)
                .HasColumnName("className");
            entity.Property(e => e.CompanyId).HasColumnName("companyId");
            entity.Property(e => e.HeadingName).HasColumnName("headingName");
            entity.Property(e => e.Rate)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("rate");
            entity.Property(e => e.Type).HasMaxLength(50);
        });
        modelBuilder.Entity<GstVatAccountListDto>().HasNoKey();
        modelBuilder.Entity<SaleHeading>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SaleHead__3214EC07038C0EA7");

            entity.ToTable("SaleHeading", "dbo");

            entity.Property(e => e.NumberStartFrom)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Prefix)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Suffix)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TaxOnSaleType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TypeOfSale)
                .HasMaxLength(100)
                .IsUnicode(false);
        });
        modelBuilder.Entity<SaleHeadingById>().HasNoKey();
        modelBuilder.Entity<SaleInvoice>(entity =>
        {
            entity.HasKey(e => e.SaleInvoiceId).HasName("PK__SaleInvo__DB4318BC3C5E9080");

            entity.ToTable("SaleInvoice", "dbo");

            entity.Property(e => e.CentralGst).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ClaimDate).HasColumnType("datetime");
            entity.Property(e => e.CreditDays).HasMaxLength(50);
            entity.Property(e => e.Dated).HasMaxLength(50);
            entity.Property(e => e.DocuThru).HasMaxLength(100);
            entity.Property(e => e.DueDate).HasMaxLength(50);
            entity.Property(e => e.EcomGstin)
                .HasMaxLength(50)
                .HasColumnName("EcomGSTIN");
            entity.Property(e => e.EntrBy).HasMaxLength(50);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EwayNo).HasMaxLength(50);
            entity.Property(e => e.ExtraAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("extraAmount");
            entity.Property(e => e.FinalDestination).HasMaxLength(100);
            entity.Property(e => e.FormNo).HasMaxLength(50);
            entity.Property(e => e.Freight).HasMaxLength(50);
            entity.Property(e => e.Grno)
                .HasMaxLength(50)
                .HasColumnName("GRNo");
            entity.Property(e => e.InvoiceDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceHeading).HasMaxLength(200);
            entity.Property(e => e.InvoiceNo).HasMaxLength(50);
            entity.Property(e => e.LocalGst).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OrderNo).HasMaxLength(50);
            entity.Property(e => e.Packages).HasMaxLength(100);
            entity.Property(e => e.PackingNo).HasMaxLength(50);
            entity.Property(e => e.PortDischarge).HasMaxLength(100);
            entity.Property(e => e.PortLoading).HasMaxLength(100);
            entity.Property(e => e.PvtMark).HasMaxLength(100);
            entity.Property(e => e.Rgpno)
                .HasMaxLength(50)
                .HasColumnName("RGPNo");
            entity.Property(e => e.RoundAndTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ShBno)
                .HasMaxLength(50)
                .HasColumnName("ShBNo");
            entity.Property(e => e.ShipDate).HasMaxLength(50);
            entity.Property(e => e.ShipPartNo).HasMaxLength(50);
            entity.Property(e => e.ShippingBillNo).HasMaxLength(50);
            entity.Property(e => e.Station).HasMaxLength(100);
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SwachBharat).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TaxableSale).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Tcs).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Transport).HasMaxLength(100);
            entity.Property(e => e.Value).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Value1).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Vehicle).HasMaxLength(50);
            entity.Property(e => e.Weight).HasMaxLength(50);
        });
        modelBuilder.Entity<NextInvoiceNoDto>().HasNoKey();
        modelBuilder.Entity<NextInvoiceHeadingNoDto>().HasNoKey();
        modelBuilder.Entity<SaleInvoiceDetail>(entity =>
        {
            entity.HasKey(e => e.SaleInvoiceDetailId).HasName("PK__SaleInvo__069C4C09D2C5F7E1");

            entity.ToTable("SaleInvoiceDetail", "dbo");

            entity.Property(e => e.ArtNo).HasMaxLength(50);
            entity.Property(e => e.Barcode).HasMaxLength(100);
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.DiscAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DiscPer).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Hsn)
                .HasMaxLength(50)
                .HasColumnName("HSN");
            entity.Property(e => e.Mrate)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("MRate");
            entity.Property(e => e.Pack1).HasMaxLength(50);
            entity.Property(e => e.Pack2).HasMaxLength(50);
            entity.Property(e => e.Qty).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Rate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Remarks).HasMaxLength(200);
            entity.Property(e => e.RowTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Size).HasMaxLength(50);
            entity.Property(e => e.TaxPercent).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TaxTableRowSubTotal)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("taxTableRowSubTotal");
        });

        modelBuilder.Entity<ScreenManagement>(entity =>
        {
            entity.HasKey(e => e.ScreenId).HasName("PK__ScreenMa__0AB60FA5AF459436");

            entity.ToTable("ScreenManagement", "dbo");
            entity.Property(e => e.HSNPurchase).HasColumnName("HSNPurchase");
            entity.Property(e => e.HSNSale).HasColumnName("HSNSale");
            entity.Property(e => e.mRatePurchase).HasColumnName("mRatePurchase");
            entity.Property(e => e.mRateSale).HasColumnName("mRateSale");
        });

        modelBuilder.Entity<SubUserDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SubUserD__3214EC07750819CD");

            entity.ToTable("SubUserDetails", "dbo");

            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Permissions).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<TaxTableMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__taxTable__3213E83FFF944BFE");

            entity.ToTable("taxTableMaster", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("companyId");
            entity.Property(e => e.ExciseApplicabe).HasColumnName("exciseApplicabe");
            entity.Property(e => e.GstApplicabeCentral).HasColumnName("gstApplicabeCentral");
            entity.Property(e => e.GstApplicabeCentralCalculateOn)
                .HasMaxLength(50)
                .HasColumnName("gstApplicabeCentralCalculateOn");
            entity.Property(e => e.GstApplicabeCentralName).HasColumnName("gstApplicabeCentralName");
            entity.Property(e => e.GstApplicabeCentralRate)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("gstApplicabeCentralRate");
            entity.Property(e => e.GstApplicabeLocal).HasColumnName("gstApplicabeLocal");
            entity.Property(e => e.GstApplicabeLocalCalculateOn)
                .HasMaxLength(50)
                .HasColumnName("gstApplicabeLocalCalculateOn");
            entity.Property(e => e.GstApplicabeLocalName).HasColumnName("gstApplicabeLocalName");
            entity.Property(e => e.GstApplicabeLocalRate)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("gstApplicabeLocalRate");
            entity.Property(e => e.SalePurcAccountName).HasColumnName("salePurcAccountName");
            entity.Property(e => e.SelectType)
                .HasMaxLength(50)
                .HasColumnName("selectType");
            entity.Property(e => e.SubTotalTax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SwachBhartApplicable).HasColumnName("swachBhartApplicable");
            entity.Property(e => e.SwachBhartApplicableCalculateOn)
                .HasMaxLength(50)
                .HasColumnName("swachBhartApplicableCalculateOn");
            entity.Property(e => e.SwachBhartApplicableName).HasColumnName("swachBhartApplicableName");
            entity.Property(e => e.SwachBhartApplicableRate)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("swachBhartApplicableRate");
            entity.Property(e => e.TcsApplicabe).HasColumnName("tcsApplicabe");
            entity.Property(e => e.TcsApplicabeCalculateOn)
                .HasMaxLength(50)
                .HasColumnName("tcsApplicabeCalculateOn");
            entity.Property(e => e.TcsApplicabeName).HasColumnName("tcsApplicabeName");
            entity.Property(e => e.TcsApplicabeRate)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("tcsApplicabeRate");
            entity.Property(e => e.Totalax)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("totalax");
            entity.Property(e => e.UnderVatReturn)
                .HasMaxLength(50)
                .HasColumnName("underVatReturn");
        });

        modelBuilder.Entity<TbAccountMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbAccoun__DED88B1CC4087A2A");

            entity.ToTable("tbAccountMaster", "dbo");

            entity.Property(e => e.Id).HasColumnName("_id");
            entity.Property(e => e.AccountName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("accountName");
            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.AdharNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("adharNo");
            entity.Property(e => e.AgentAndAreaName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("agentAndAreaName");
            entity.Property(e => e.AgentId).HasColumnName("agentId");
            entity.Property(e => e.BankName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("bankName");
            entity.Property(e => e.BasicLimit)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("basicLimit");
            entity.Property(e => e.CinNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cinNo");
            entity.Property(e => e.CityId).HasColumnName("cityId");
            entity.Property(e => e.ClosingBalance)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("closingBalance");
            entity.Property(e => e.ClosingBalanceType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("closingBalanceType");
            entity.Property(e => e.ClsBalance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("clsBalance");
            entity.Property(e => e.CompanyId).HasColumnName("companyId");
            entity.Property(e => e.ContectName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("contectName");
            entity.Property(e => e.ContectNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("contectNo");
            entity.Property(e => e.CreditDay).HasColumnName("creditDay");
            entity.Property(e => e.CreditLimit)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("creditLimit");
            entity.Property(e => e.EComNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("eComNo");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.ExpHsnCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("expHsnCode");
            entity.Property(e => e.GroupId).HasColumnName("groupId");
            entity.Property(e => e.Gst)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("gst");
            entity.Property(e => e.GstVat).HasColumnName("gstVat");
            entity.Property(e => e.GstVatReturn).HasColumnName("gstVatReturn");
            entity.Property(e => e.IeCodeNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ieCodeNo");
            entity.Property(e => e.IfscCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ifscCode");
            entity.Property(e => e.InttRate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("inttRate");
            entity.Property(e => e.IsItTransport).HasColumnName("isItTransport");
            entity.Property(e => e.IsTcsCompulsary).HasColumnName("isTcsCompulsary");
            entity.Property(e => e.MaintBillWise).HasColumnName("maintBillWise");
            entity.Property(e => e.OpBalance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("opBalance");
            entity.Property(e => e.OpeningBalance)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("openingBalance");
            entity.Property(e => e.OpeningBalanceType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("openingBalanceType");
            entity.Property(e => e.Pan)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("pan");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.RegdType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("regdType");
            entity.Property(e => e.State).HasColumnName("state");
            entity.Property(e => e.StateUser)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("stateUser");
            entity.Property(e => e.TanNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tanNo");
            entity.Property(e => e.TaxFormName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("taxFormName");
            entity.Property(e => e.TcsLimit)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tcsLimit");
            entity.Property(e => e.TdsApplicabe).HasColumnName("tdsApplicabe");
            entity.Property(e => e.TradeType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tradeType");
            entity.Property(e => e.TransportId).HasColumnName("transportId");
            entity.Property(e => e.TransportMode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("transportMode");
            entity.Property(e => e.TransportRate)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("transportRate");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("zipCode");
        });

        modelBuilder.Entity<TaxTableDetailsDto>().HasNoKey();

        modelBuilder.Entity<TbAgentMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbAgentM__3214EC0766511006");

            entity.ToTable("tbAgentMaster");

            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.AgentName).HasMaxLength(200);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EnterBy).HasMaxLength(200);
            entity.Property(e => e.PanNo).HasMaxLength(20);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.PinCode).HasMaxLength(10);
        });
        modelBuilder.Entity<AddEditAgentResult>().HasNoKey();
        modelBuilder.Entity<TbAppearInMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbAppear__3213E83F16774BC1");

            entity.ToTable("tbAppearInMaster");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TbCategoryMaster>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__tbCatego__23CAF1D849004C6F");

            entity.ToTable("tbCategoryMaster", "dbo");

            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasColumnName("categoryName");
            entity.Property(e => e.CategoryType)
                .HasMaxLength(100)
                .HasColumnName("categoryType");
            entity.Property(e => e.CompanyId).HasColumnName("companyId");
        });

        modelBuilder.Entity<TbCityMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbCityMa__3214EC07B160933E");

            entity.ToTable("TbCityMaster", "dbo");

            entity.Property(e => e.City).HasMaxLength(100);

            entity.HasOne(d => d.StateNavigation).WithMany(p => p.TbCityMasters)
                .HasForeignKey(d => d.State)
                .HasConstraintName("FK__TbCityMas__State__0B91BA14");
        });

        modelBuilder.Entity<CityDetails>(entity =>
        {
            entity.HasNoKey();
            entity.ToView(null);
        });
        modelBuilder.Entity<CityDetailsDto>(entity =>
        {
            entity.HasNoKey();
            entity.ToView(null);
        });

        modelBuilder.Entity<TbClientLoginMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbClient__3214EC079AC8218F");

            entity.ToTable("TbClientLoginMaster", "dbo");

            entity.HasIndex(e => e.PhoneNumber, "UQ__TbClient__85FB4E38A69E7A1F").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.ClientName).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Package).HasMaxLength(200);
            entity.Property(e => e.PackageAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Zip).HasMaxLength(20);

            entity.HasOne(d => d.CityNavigation).WithMany(p => p.TbClientLoginMasters)
                .HasForeignKey(d => d.City)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TbClientLo__City__0C85DE4D");
        });

        modelBuilder.Entity<AddEditClientRequest>(entity =>
        {
            entity.HasNoKey();
            entity.ToView(null);
        });

        modelBuilder.Entity<ClientDetailsDto>(entity =>
        {
            entity.HasNoKey();
            entity.ToView(null);
        });
        modelBuilder.Entity<ClientDetailsResponse>(entity =>
        {
            entity.HasNoKey();
            entity.ToView(null);
        });

        modelBuilder.Entity<TbCompanyDetailsMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbCompan__3214EC0707845526");

            entity.ToTable("TbCompanyDetailsMaster", "dbo");

            entity.Property(e => e.CompanyName).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Gstno).HasMaxLength(50);
            entity.Property(e => e.Panno).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Sinno).HasMaxLength(50);
            entity.Property(e => e.Zip).HasMaxLength(20);
        });

        modelBuilder.Entity<CompanyDetailsDto>().HasNoKey();
        modelBuilder.Entity<CompanyIdResponse>().HasNoKey();
        modelBuilder.Entity<SubUserLoginDto>().HasNoKey();
        modelBuilder.Entity<GroupDto>().HasNoKey();
        modelBuilder.Entity<AccountListDto>().HasNoKey();
        modelBuilder.Entity<AccountByIdDto>().HasNoKey();
        modelBuilder.Entity<GroupMasterZero>().HasNoKey();
        modelBuilder.Entity<ItemGroupDTOComanyId>().HasNoKey();
        
        modelBuilder.Entity<AccountDuplicateCheckDto>()
    .HasNoKey();
        modelBuilder.Entity<TbGroupCategoryMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbGroupC__3213E83F8F32B795");

            entity.ToTable("tbGroupCategoryMaster");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TbGroupMaster>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PK__tbGroupM__88C1034DB62141E5");

            entity.ToTable("tbGroupMaster", "dbo");

            entity.Property(e => e.GroupId).HasColumnName("groupId");
            entity.Property(e => e.AnnexureNo)
                .HasMaxLength(100)
                .HasColumnName("annexureNo");
            entity.Property(e => e.AppearIn).HasColumnName("appearIn");
            entity.Property(e => e.GroupCategory).HasColumnName("groupCategory");
            entity.Property(e => e.GroupName)
                .HasMaxLength(200)
                .HasColumnName("groupName");
            entity.Property(e => e.UnderGroup).HasColumnName("underGroup");
        });

        modelBuilder.Entity<TbHeadingNameMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbHeadin__3213E83FAA731CE3");

            entity.ToTable("tbHeadingNameMaster", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("companyId");
            entity.Property(e => e.EntrBy)
                .HasMaxLength(100)
                .HasColumnName("entrBy");
            entity.Property(e => e.HeadingName)
                .HasMaxLength(100)
                .HasColumnName("headingName");
        });
        modelBuilder.Entity<CommonSpResponse>().HasNoKey();
        modelBuilder.Entity<TbItemGroupMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbItemGr__3214EC07C473A4BB");

            entity.ToTable("tbItemGroupMaster", "dbo");

            entity.Property(e => e.CompanyId).HasColumnName("companyId");
            entity.Property(e => e.ItemGroup).HasColumnName("itemGroup");
            entity.Property(e => e.ItemGroupName)
                .HasMaxLength(200)
                .HasColumnName("itemGroupName");
        });
        modelBuilder.Entity<ItemGroupMasterById>().HasNoKey();
        modelBuilder.Entity<TbItemMaster>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__tbItemMa__727E838BEB5372DF");

            entity.ToTable("tbItemMaster", "dbo");

            entity.Property(e => e.BarCodeType)
                .HasMaxLength(100)
                .HasColumnName("barCodeType");
            entity.Property(e => e.CessQty).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CessRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CgstSgstPurchase).HasMaxLength(200);
            entity.Property(e => e.CgstSgstSale).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EnterBy).HasMaxLength(100);
            entity.Property(e => e.EnteredOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Hsn).HasMaxLength(100);
            entity.Property(e => e.IgstPurchase).HasMaxLength(200);
            entity.Property(e => e.IgstSaleName).HasMaxLength(200);
            entity.Property(e => e.ItemBarCodeOrPartNo)
                .HasMaxLength(200)
                .HasColumnName("itemBarCodeOrPartNo");
            entity.Property(e => e.ItemGroupId).HasColumnName("itemGroupId");
            entity.Property(e => e.ItemName).HasMaxLength(200);
            entity.Property(e => e.ItemType).HasMaxLength(100);
            entity.Property(e => e.MrpRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OpeningStock).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OpeningStockRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OpeningStockValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Packing)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("packing");
            entity.Property(e => e.PackingInUnit)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("packingInUnit");
            entity.Property(e => e.SaleRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TaxRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Unit).HasMaxLength(50);
        });
        modelBuilder.Entity<ItemMasterListDto>().HasNoKey();
        modelBuilder.Entity<DbMessageDto>().HasNoKey();
        modelBuilder.Entity<ApiResponse>().HasNoKey();

        modelBuilder.Entity<PurchaseInvoiceApiResponse>().HasNoKey();

        
        modelBuilder.Entity<PaymentReceiptResponse>().HasNoKey();
        modelBuilder.Entity<TbStateMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TbStateM__3214EC0701F1C507");

            entity.ToTable("TbStateMaster", "dbo");

            entity.Property(e => e.State).HasMaxLength(100);
            entity.Property(e => e.StateCode).HasMaxLength(50);
        });

        modelBuilder.Entity<TbTransportMaster>(entity =>
        {
            entity.HasKey(e => e.TransportId).HasName("PK__tbTransp__591756BF111FAC9E");

            entity.ToTable("tbTransportMaster", "dbo");

            entity.Property(e => e.TransportId).HasColumnName("transportId");
            entity.Property(e => e.CompanyId).HasColumnName("companyId");
            entity.Property(e => e.GstNo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("gstNo");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<TbUnitMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbUnitMa__3214EC0791E74CD1");

            entity.ToTable("tbUnitMaster", "dbo");

            entity.Property(e => e.CompanyId).HasColumnName("companyId");
            entity.Property(e => e.Decimal)
                .HasMaxLength(50)
                .HasColumnName("decimal");
            entity.Property(e => e.Quantity)
                .HasMaxLength(50)
                .HasColumnName("quantity");
            entity.Property(e => e.UnitName)
                .HasMaxLength(100)
                .HasColumnName("unitName");
        });

        modelBuilder.Entity<TbUserGroupMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbUserGr__3214EC07A8F0F90D");

            entity.ToTable("tbUserGroupMapping", "dbo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
