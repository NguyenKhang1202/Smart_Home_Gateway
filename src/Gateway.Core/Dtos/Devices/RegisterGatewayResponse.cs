namespace Gateway.Core.Dtos.Devices
{
    public class RegisterGatewayResponse
    {
        public object? data { get; set; }
        public string deviceCode { get; set; }
        public long time { get; set; }
        public string mac { get; set; }
        public string to { get; set; }
        public string from { get; set; }
        public string type { get; set; }
    }

    public class DataInside
    {
        public int? code { get; set; }
        public string? deviceCode { get; set; }
        public string? mac { get; set; }
    }
}
