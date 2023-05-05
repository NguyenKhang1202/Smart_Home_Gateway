namespace Gateway.Core.Dtos.Homes
{
    public class UpdateHomeInputDto
    {
        public string Id { get; set; }
        public string? SmartHomeCode { get; set; }
        public string? Name { get; set; }
        public string? Properties { get; set; }
        public string? Address { get; set; }
    }
}
