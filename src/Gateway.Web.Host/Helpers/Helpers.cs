namespace Gateway.Web.Host.Helpers
{
    public static class HelpersClass
    {
        public static long GetCurrentTimeByLong()
        {
            return (long)(((DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds) / 1000);
        }
    }
}
