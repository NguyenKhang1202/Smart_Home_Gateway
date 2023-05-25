namespace Gateway.Core.Dtos.Rooms
{
    public class GetAllRoomsInputDto
    {
        public int MaxResultCount { get; set; } = 10;
        public int SkipCount { get; set; } = 0;
    }
}
