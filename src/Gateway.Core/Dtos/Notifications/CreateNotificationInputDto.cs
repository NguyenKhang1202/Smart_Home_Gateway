namespace Gateway.Core.Dtos.Notifications
{
    public class CreateNotificationInputDto
    {
        public string TenantId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
