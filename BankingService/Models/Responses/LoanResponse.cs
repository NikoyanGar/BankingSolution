namespace BankingService.Models.Responses
{
    public class LoanResponse
    {
        public List<LoanEntity>? History { get; set; }
        public decimal FeeCharged { get; set; }
    }
}
