namespace BankingService.Models
{
    public class LoanDecision
    {
        public string? ClientId { get; set; }
        public decimal RequestedAmount { get; set; }
        public string? Decision { get; set; }
        public string? Reason { get; set; }
    }
}
