namespace Ignite2019.IoT.Orleans.Events
{
    public class ControlEvent : DeviceEvent
    {
        public string Command { get; set; }

        public int? UserId { get; set; }
    }
}