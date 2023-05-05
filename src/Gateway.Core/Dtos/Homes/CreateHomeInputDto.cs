namespace Gateway.Core.Dtos.Homes
{
    public class CreateHomeInputDto
    {
        public string TenantId { get; set; }
        public string UserId { get; set; }
        public string? SmartHomeCode { get; set; }
        public string? Name { get; set; }
        public string? Properties { get; set; }
        public string? Address { get; set; }
        public List<string> ImagesUrl { get; set; }
    }
}
