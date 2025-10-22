namespace UserService.Options
{
    public sealed class DbConnectionOptions
    {
        public string Host { get; set; } = "0.0.0.0";
        public int Port { get; set; } = 5432;
        public string Database { get; set; } = "FirstTestDB";
        public string Username { get; set; } = "postgres";
        public string Password { get; set; } = "p@$$w0rd";
    }
}
