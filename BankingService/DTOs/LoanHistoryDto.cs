namespace BankingService.DTOs
{
    public class LoanHistoryDto
    {
        public string? ClientId { get; set; }
        public decimal Amount { get; set; }
        public bool Approved { get; set; }
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public string? LoanId { get; set; }
        public DateTime DateIssued { get; set; }
        public bool IsRepaid { get; set; }
    }
}
