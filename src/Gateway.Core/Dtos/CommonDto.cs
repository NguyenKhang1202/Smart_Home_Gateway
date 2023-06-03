namespace Gateway.Core.Dtos
{
    public class ResponseDto
    {
        public object? Data { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int? TotalCount { get; set; } = null;
    }

    public class UserInfo
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string TenantId { get; set; }
    }


}
