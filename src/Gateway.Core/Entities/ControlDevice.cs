namespace Gateway.Core.Entities
{
    public class ControlDevice
    {
        public int Status { get; set; }
        public int Mode { get; set; }
        public int Direction { get; set; }
        public int Speed { get; set; }
        public int Intensity { get; set; }
    }

    public enum STATUS
    {
        ON = 1,
        OFF = 2,
    }

    public enum MODE
    {
        AUTO = 1,
        MANUAL = 2,
    }
}
