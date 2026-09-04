namespace softDataApi.Models
{
    public class AccountLedgerDto
    {
        public DateTime? Date { get; set; }

        public string? Particulars { get; set; }

        public string? VoucherType { get; set; }

        public string? VoucherNo { get; set; }

        public decimal? Qty { get; set; }

        public string? Items { get; set; }

        public decimal? Debit { get; set; }

        public decimal? Credit { get; set; }

        public decimal? Balance { get; set; }

        public string? BalanceType { get; set; }
    }
}
