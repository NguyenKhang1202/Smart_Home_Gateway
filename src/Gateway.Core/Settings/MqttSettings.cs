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
    public class MqttDataReceiveDusun
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Type { get; set; }
        public string Mac { get; set; }
        // public long Time { get; set; } = (long)(((DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds) / 1000);
        public long Time { get; set; }
        public string DeviceCode { get; set; }
        // public DataInside Data { get; set; }
        public object? Data { get; set; }
    }
    public class DataInside
    {
        public string? Id { get; set; }
        public int? Code { get; set; }
        public string? Attribute { get; set; }
        public string? Mac { get; set; }
        public ValueDataInside? Value { get; set; }
    }
    public class ValueDataInside
    {
        public string? Version { get; set; }
        public string? Model { get; set; }
        public string? Factory { get; set; }
        public long? Current_time { get; set; }
        public long? Uptime { get; set; }
        public string? UplinkType { get; set; }
        public List<DeviceDusunGateway>? Device_list { get; set; }
    }
    public class DeviceDusunGateway
    {
        public string? Mac { get; set; }
        public string? Mcuversion { get; set; }
        public string? Type { get; set; }
        public List<EpDto>? EpList { get; set; }
        public string? Subtype { get; set; }
        public string? Version { get; set; }
        public string? Zigversion { get; set; }
        public string? Model { get; set; }
        public int? Online { get; set; }
        public int? Battery { get; set; }
        public string? ModelStr { get; set; }
        public string? State { get; set; }
        public int? Rssi { get; set; }
    }
    public class EpDto
    {
        public string? Type { get; set; }
        public string? Ep { get; set; }
    }
}
