namespace Ignite2019.IoT.Orleans.Grains.EventSource
{
    public class ReportEvent : DeviceEvent
    {
        public string Data { get; set; }
    }
}