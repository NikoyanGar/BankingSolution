namespace BankingService.Models.Requests
{
    public class EvaluateLoanRequest
    {
        public string? ClientId { get; set; }
        public decimal Amount { get; set; }
    }
}
