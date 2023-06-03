namespace Gateway.Core.Dtos.Devices
{
    public class CreateDeviceInputDto
    {
        public string TenantId { get; set; }
        public string? DeviceCode { get; set; }
        public string? Name { get; set; }
        public int Type { get; set; }
        public string HomeDeviceId { get; set; }
        public string RoomId { get; set; }
        public string? Properties { get; set; }
        public string GatewayCode { get; set; }
        public string? Producer { get; set; }
        public List<string> ImagesUrl { get; set; }
    }
}
