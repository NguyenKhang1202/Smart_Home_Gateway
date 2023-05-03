using System.ComponentModel.DataAnnotations;

namespace Gateway.Core.Dtos.Authentications
{
    public class RegisterInputDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public string Fullname { get; set; }
    }
}
