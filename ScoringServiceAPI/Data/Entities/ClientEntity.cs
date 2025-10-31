namespace ScoringServiceAPI.Data.Entities
{
    public class ClientEntity
    {
        public int Id { get; set; }
        public string? ClientId { get; set; }
        public int Score { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
