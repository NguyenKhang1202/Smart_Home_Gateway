namespace Gateway.Application.Shared.Enums
{
    public static class DeviceEnum
    {
        public enum DEVICE_TYPE
        {
            UNKNOWN = 0,
            FLAME = 1,
            MQ2 = 2,
            DHT = 3,
            LIGHT = 4,
            PAN = 5,

            GATEWAY = 100,
        }

        public static string Topic_Control = "projects/smart_home/control";
    }
}
