namespace ScoringServiceAPI.Models.Requests
{
    public class AddScoreRequest
    {
        public string? ClientId { get; set; }
        public int Score { get; set; }
    }
}
