namespace BankingService.DTOs
{
    public class LoanResponseDto
    {
        public List<LoanHistoryDto>? History { get; set; }
        public decimal FeeCharged { get; set; }
    }
}
