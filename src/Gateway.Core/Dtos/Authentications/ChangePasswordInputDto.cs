namespace Gateway.Core.Dtos.Authentications
{
    public class ChangePasswordInputDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
