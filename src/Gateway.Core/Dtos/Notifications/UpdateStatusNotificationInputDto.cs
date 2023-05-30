namespace Gateway.Core.Dtos.Notifications
{
    public class UpdateStatusNotificationInputDto
    {
        public string Id { get; set; }
        public int CurrentStatus { get; set; }
        public int UpdateStatus { get; set; }
    }
}
