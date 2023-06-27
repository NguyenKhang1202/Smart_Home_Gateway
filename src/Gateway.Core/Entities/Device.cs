using Gateway.Core.Entities.Base;

namespace Gateway.Core.Entities
{
    public class Device : MongoEntity
    {
        public string TenantId { get; set; }
        public string UserId { get; set; }
        public string DeviceCode { get; set; }
        public string Name { get; set; }
        public string Properties { get; set; }
        public int Type { get; set; }
        public string Producer { get; set; }
        public string HomeDeviceId { get; set; }
        public string RoomId { get; set; }
        public string GatewayCode { get; set; }
        public List<string> ImagesUrl { get; set; }
        public ControlDevice Control { get; set; }
        public List<int> Data { get; set; }
        public string? MacAddress { get; set; }
        public List<DataDusun>? DataDusuns { get; set; }
    }
    public class DataDusun
    {
        public int Value { get; set; }
        public int EndPoint { get; set; }
    }
}
