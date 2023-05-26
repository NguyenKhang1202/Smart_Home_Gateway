namespace Gateway.Core.Dtos.Devices
{
    public class GetAllDevicesInputDto
    {
        public int MaxResultCount { get; set; } = 10;
        public int SkipCount { get; set; } = 0;
        public string? HomeDeviceId { get; set; }
        public string? RoomId { get; set; }
    }
}
