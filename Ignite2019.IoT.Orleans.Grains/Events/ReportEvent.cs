namespace Ignite2019.IoT.Orleans.Events
{
    public class ReportEvent : DeviceEvent
    {
        public ReportEvent(string data)
        {
            Data = data;
        }
        public string Data { get; set; }
    }
}