namespace Gateway.Core.Dtos.Homes
{
    public class GetAllHomesInputDto
    {
        public int MaxResultCount { get; set; } = 10;
        public int SkipCount { get; set; } = 0;
        public string UserId { get; set; }
    }
}
