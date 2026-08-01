namespace softDataApi.Models
{
    public class tbNarration
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string? Narration { get; set; }
    }

}
public class ResponseMessageNarration
{
    public int Success { get; set; }
    public string? Message { get; set; }
}