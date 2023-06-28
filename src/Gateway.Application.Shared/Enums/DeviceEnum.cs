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
            DOOR = 6,
            SWITCH = 7,

            GATEWAY = 100,
            GATEWAY_DUSUN = 101,
        }

        public static string TOPIC_CONTROL = "projects/smart_home/control";
        public static string TOPIC_DATA = "projects/smart_home/data";
        public static string TOPIC_GATEWAY_SUBSCRIBE = "t";
        public static string TOPIC_GATEWAY_PUBLISH_PREFIX = "t";

        public static string DEVICE_PUBLISH_BLE = "BLE";
        public static string DEVICE_PUBLISH_GATEWAY = "GATEWAY";
        public static string DEVICE_PUBLISH_GREENPOWER = "GREENPOWER";
        public static string DEVICE_PUBLISH_ZWAVE = "ZWAVE";
        public static string DEVICE_PUBLISH_CLOUD = "CLOUD";
        public static string DEVICE_PUBLISH_NXP = "NXP";

        public static string TYPE_COMMAND = "cmdResult";
        public static string TYPE_REPORT = "reportAttribute";
        public static string TYPE_REGISTER_REQ = "registerReq";
        public static string TYPE_REGISTER_RESP = "registerResp";
        public static string TYPE_CMD = "cmd";

        public static string COMMAND_ADD_DEVICE = "setAttribute";
        public static string COMMAND_DELETE_DEVICE = "setAttribute";
        public static string COMMAND_CONTROL_DEVICE = "setAttribute";

        public static string ATTRIBUTE_ADD_DEVICE = "mod.add_device";
        public static string ATTRIBUTE_DELETE_DEVICE = "mod.del_device";
        public static string ATTRIBUTE_CONTROL_DEVICE = "device.onoff";
    }
}
