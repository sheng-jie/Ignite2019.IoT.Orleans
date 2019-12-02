using Ignite2019.IoT.Orleans.Model;

namespace Ignite2019.IoT.Orleans.Events
{
    public class OnlineEvent : DeviceEvent
    {
        public OnlineEvent()
        {
            base.EventType = EventType.Online;
        }

    }
}