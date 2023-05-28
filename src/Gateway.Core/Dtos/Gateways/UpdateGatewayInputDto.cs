namespace Gateway.Core.Dtos.Gateways
{
    public class UpdateGatewayInputDto
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? DeviceCode { get; set; }
        public int TenantId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HomeId { get; set; }
        public string Key { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
    }
}
