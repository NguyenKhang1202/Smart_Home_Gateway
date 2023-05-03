using System.ComponentModel.DataAnnotations;

namespace Gateway.Core.Dtos.Authentications
{
    public class RegisterInputDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int RoleId { get; set; }
        public int TenantId { get; set; }
    }
}
