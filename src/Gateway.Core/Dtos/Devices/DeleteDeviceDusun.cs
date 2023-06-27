namespace Gateway.Core.Dtos.Devices
{
    public class DeleteDeviceDusun
    {
        public DeleteDeviceDataInside data { get; set; }
        public string from { get; set; }
        public string mac { get; set; }
        public long messageId { get; set; }
        public long time { get; set; }
        public string to { get; set; }
        public string type { get; set; }
    }
    public class DeleteDeviceDataInside
    {
        public ArgumentsInside? arguments { get; set; }
        public string id { get; set; }
        public string command { get; set; }
    }
}
