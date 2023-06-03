namespace Gateway.Core.Dtos.Notifications
{
    public class GetAllNotificationsInputDto
    {
        public int MaxResultCount { get; set; } = 10;
        public int SkipCount { get; set; } = 0;
    }
}
