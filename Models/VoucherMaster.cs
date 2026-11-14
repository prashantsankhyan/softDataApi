namespace softDataApi.Models
{
    public class VoucherMaster
    {
        public int? Id { get; set; }

        public int CompanyId { get; set; }

        public int Type { get; set; }

        public string? Date { get; set; }

        public string? Day { get; set; }

        public string? SeriaNumber { get; set; }

        public decimal DTotal { get; set; }

        public decimal CTotal { get; set; }

        public List<VoucherDetailModel>? Details { get; set; }
    }
    public class VoucherDetailModel
    {
        public int AccountId { get; set; }

        public string? Station { get; set; }

        public string? Narration { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
    public class VoucherResponse
    {
        public int Success { get; set; }

        public string? Message { get; set; }

        public int? MasterId { get; set; }

        public decimal DTotal { get; set; }

        public decimal CTotal { get; set; }
    }

    public class VoucherDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int Type { get; set; }
        public DateTime Date { get; set; }
        public string? Day { get; set; }
        public string? SeriaNumber { get; set; }
        public decimal DTotal { get; set; }
        public decimal CTotal { get; set; }

        public List<VoucherDetailDto> Details { get; set; } = new();
    }

    public class VoucherDetailDto
    {
        public int DetailId { get; set; }
        public int MasterId { get; set; }
        public int AccountId { get; set; }

        public string? AccountName { get; set; }
        public string? Station { get; set; }
        public string? Narration { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
    public class JournalVoucherType
    {
        public int VoucherId { get; set; }

        public string Type { get; set; } = string.Empty;
    }
}
