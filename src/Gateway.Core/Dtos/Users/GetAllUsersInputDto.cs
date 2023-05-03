namespace Gateway.Core.Dtos.Users
{
    public class GetAllUsersInputDto
    {
        public int TenantId { get; set; }
        public int MaxResultCount { get; set; } = 10;
        public int SkipCount { get; set; } = 0;
    }
}
