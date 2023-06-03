namespace Gateway.Core.Dtos.Rooms
{
    public class CreateRoomInputDto
    {
        public string TenantId { get; set; }
        public string? RoomCode { get; set; }
        public string? Name { get; set; }
        public string HomeId { get; set; }
        public string? Properties { get; set; }
        public List<string> ImagesUrl { get; set; }
    }
}
