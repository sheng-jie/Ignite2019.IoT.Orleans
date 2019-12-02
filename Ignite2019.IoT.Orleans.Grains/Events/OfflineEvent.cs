using Ignite2019.IoT.Orleans.Model;

namespace Ignite2019.IoT.Orleans.Events
{
    public class OfflineEvent : DeviceEvent
    {
        public OfflineEvent()
        {
            base.EventType = EventType.Offline;
        }
    }
}