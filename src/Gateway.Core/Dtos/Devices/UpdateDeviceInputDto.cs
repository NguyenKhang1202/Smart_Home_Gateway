namespace Gateway.Core.Dtos.Devices
{
    public class UpdateDeviceInputDto
    {
        public string Id { get; set; }
        public string? DeviceCode { get; set; }
        public string? Name { get; set; }
        public string? Properties { get; set; }
        public List<string> ImagesUrl { get; set; }
    }
}
