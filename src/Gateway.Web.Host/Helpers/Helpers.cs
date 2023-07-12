namespace Gateway.Web.Host.Helpers
{
    public static class HelpersClass
    {
        public static long GetCurrentTimeByLong()
        {
            return (long)(((DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds) / 1000);
        }
        public static string GenerateOrderCode()
        {
            Random random = new();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
