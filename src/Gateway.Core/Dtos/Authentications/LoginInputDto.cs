using System.ComponentModel.DataAnnotations;

namespace Gateway.Core.Dtos.Authentications
{
    public class LoginInputDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
