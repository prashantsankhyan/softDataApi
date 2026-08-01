namespace softDataApi.Models
{
    public class paymentReceiptVocher
    {
        public int? PaymentReceiptId { get; set; }
        public int AccountName { get; set; }
        public string? ReceiptDate { get; set; }
        public string? DayName { get; set; }
        public string? VoucherNo { get; set; }
        public string? NarrationForSingleAccount { get; set; }
        public int CompanyId { get; set; }
       

        public List<PaymentReceiptDetailRequest>? Details { get; set; }
    }
    public class PaymentReceiptDetailRequest
    {
        public int AccountId { get; set; }
        public string? Station { get; set; }
        public string? Narration { get; set; }
        public decimal? Amount { get; set; }
        public string? CrOrDr { get; set; }
    }
    public class PaymentReceiptResponse
    {
        public int Success { get; set; }
        public string? Message { get; set; }
        public int? PaymentReceiptId { get; set; }
    }
}
