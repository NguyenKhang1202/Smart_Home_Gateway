using Gateway.Core.Entities;

namespace Gateway.Core.Settings
{
    public class MqttSettings
    {
        public string ClientId { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Payload
    {
        public string GatewayCode { get; set; }
        public string DeviceCode { get; set; }
        public ControlDevice Control { get; set; }
    }

    public class MqttDataReceive
    {
        public string Name { get; set; }
        public bool? Bool_value { get; set; }
        public string Code { get; set; }  // Device Code
        public int Type { get; set; }
        public string? Humidity { get; set; }
        public string? Temperature { get; set; }
    }
}
