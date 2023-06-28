using Gateway.Core.Entities;

namespace Gateway.Core.Dtos.Devices
{
    public class ControlDeviceInputDto
    {
        public string Id { get; set; }
        public ControlDevice Control { get; set; }
        public ControlDeviceDusun? ControlDusun { get; set; }
    }
}
