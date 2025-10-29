using System.Security.Claims;

namespace UserService
{
    public static class ClaimConstants
    {
        public const string UserId = "userid";
        public const string Name = "name";
        public const string Mail = ClaimTypes.Email;
        public const string ClientId = "clientId";
        public const string Roles = ClaimTypes.Role;
    }
}
