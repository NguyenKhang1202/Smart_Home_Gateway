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
        public int? Value { get; set; }
        public string DeviceCode { get; set; }  // Device Code
        public int Type { get; set; }
        public int? Humidity { get; set; }
        public int? Temperature { get; set; }
    }

    // Dusun
    public class MqttDataReceiveDusun
    {
        public string from { get; set; }
        public string to { get; set; }
        public string type { get; set; }
        public string mac { get; set; }
        public long time { get; set; }
        public string deviceCode { get; set; }
        public DataInside? data { get; set; }
    }
    public class DataInside
    {
        public string? id { get; set; }
        public int? code { get; set; }
        public string? attribute { get; set; }
        public string? mac { get; set; }
        public int? ep { get; set; }
        public ValueDataInside? value { get; set; }
    }
    public class ValueDataInside
    {
        public string? zone { get; set; }
        public int? value { get; set; }
        public int? ep { get; set; }
        public string? ModelStr { get; set; }
        public int? battery { get; set; }
        public string? version { get; set; }
        public string? model { get; set; }
        public string? factory { get; set; }
        public long? current_time { get; set; }
        public long? uptime { get; set; }
        public string? uplinkType { get; set; }
        public List<DeviceDusunGateway>? device_list { get; set; }

    }
    public class DeviceDusunGateway
    {
        public string? mac { get; set; }
        public string? mcuversion { get; set; }
        public string? type { get; set; }
        public List<EpDto>? epList { get; set; }
        public string? subtype { get; set; }
        public string? version { get; set; }
        public string? zigversion { get; set; }
        public string? model { get; set; }
        public int? online { get; set; }
        public int? battery { get; set; }
        public string? ModelStr { get; set; }
        public string? state { get; set; }
        public int? rssi { get; set; }
    }
    public class EpDto
    {
        public string? type { get; set; }
        public string? ep { get; set; }
    }
}
