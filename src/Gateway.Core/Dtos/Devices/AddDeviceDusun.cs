namespace Gateway.Core.Dtos.Devices
{
    public class AddDeviceDusun
    {
        public AddDeviceDataInside data { get; set; }
        public string from { get; set; }
        public string mac { get; set; }
        public long messageId { get; set; }
        public long time { get; set; }
        public string to { get; set; }
        public string type { get; set; }
    }
    public class AddDeviceDataInside
    {
        public ArgumentsInside? arguments { get; set; }
        public string id { get; set; }
        public string command { get; set; }
    }
    public class ArgumentsInside
    {
        public string attribute { get; set; }
        public int ep { get; set; }
        public ValueInside value { get; set; }
        public string mac { get; set; }
    }
    public class ValueInside
    {
        public string mac { get; set; }
    }
}
