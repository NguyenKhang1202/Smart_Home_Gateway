namespace Gateway.Core.Dtos.Users
{
    public class UpdateUserInputDto
    {
        public string Id { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
    }
}
