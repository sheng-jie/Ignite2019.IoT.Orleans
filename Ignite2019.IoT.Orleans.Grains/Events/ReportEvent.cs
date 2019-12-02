using Ignite2019.IoT.Orleans.Model;

namespace Ignite2019.IoT.Orleans.Events
{
    public class ReportEvent : DeviceEvent
    {
        public ReportEvent(string data)
        {
            Data = data;
            base.EventType = EventType.Report;
        }
        public string Data { get; set; }
    }
}