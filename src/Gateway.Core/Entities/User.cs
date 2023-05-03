using Gateway.Core.Entities.Base;

namespace Gateway.Core.Entities
{
    public class User : MongoEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public long RoleId { get; set; }
        public int TenantId { get; set; }
    }
}
