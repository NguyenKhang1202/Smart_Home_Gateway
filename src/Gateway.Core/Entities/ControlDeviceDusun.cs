namespace Gateway.Core.Entities
{
    public class ControlDeviceDusun
    {
        public int EndPoint { get; set; }
        public int Value { get; set; }
    }

    public class ControlDeviceDto
    {
        public string to { get; set; }
        public string from { get; set; }
        public string type { get; set; }
        public long time { get; set; }
        public string deviceCode { get; set; }
        public string mac { get; set; }
        public DataInside data { get; set; }
    }
    public class DataInside
    {
        public string id { get; set; }
        public string command { get; set; }
        public ArgumentsInside arguments { get; set; }

    }
    public class ArgumentsInside
    {
        public string mac { get; set; }
        public string ep { get; set; }
        public string attribute { get; set; }
        public ValueInside value { get; set; }
    }
    public class ValueInside
    {
        public int value { get; set; }
    }
}
