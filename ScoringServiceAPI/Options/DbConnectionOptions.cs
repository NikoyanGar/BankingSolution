namespace ScoringServiceAPI.Options
{
    public sealed class DbConnectionOptions
    {
        public string BaseUrl { get; set; } = "https://countriesnow.space/api/v0.1/countries/cities";
        public string Host { get; set; } = "0.0.0.0";
        public int Port { get; set; } = 5432;
        public string Database { get; set; } = "FirstTestDB";
        public string Username { get; set; } = "postgres";
        public string Password { get; set; } = "p@$$w0rd";
    }
}
