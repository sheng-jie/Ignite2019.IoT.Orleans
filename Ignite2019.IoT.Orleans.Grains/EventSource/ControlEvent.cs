namespace Ignite2019.IoT.Orleans.Grains.EventSource
{
    public class ControlEvent : DeviceEvent
    {
        public string Command { get; set; }
    }
}