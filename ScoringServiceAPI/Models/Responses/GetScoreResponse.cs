namespace ScoringServiceAPI.Models.Responses
{
    public class GetScoreResponse
    {
        public string? ClientId { get; set; }
        public int Score { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
