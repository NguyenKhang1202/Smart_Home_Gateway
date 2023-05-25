namespace Gateway.Core.Dtos.Rooms
{
    public class UpdateRoomInputDto
    {
        public string Id { get; set; }
        public string? RoomCode { get; set; }
        public string? Name { get; set; }
        public string? Properties { get; set; }
        public List<string> ImagesUrl { get; set; }
    }
}
