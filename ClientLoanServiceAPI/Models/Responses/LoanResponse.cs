namespace ClientLoanServiceAPI.Models.Responses
{
    public class LoanResponse
    {
        public List<LoanHistory>? History { get; set; }
        public decimal FeeCharged { get; set; }
    }
}
