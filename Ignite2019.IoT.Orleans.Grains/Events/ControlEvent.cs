using Ignite2019.IoT.Orleans.Model;

namespace Ignite2019.IoT.Orleans.Events
{
    public class ControlEvent : DeviceEvent
    {
        public ControlEvent(ControlCommand command)
        {
            this.Command = command;
        }
        public ControlCommand Command { get; set; }

        public int? UserId { get; set; }
    }
}