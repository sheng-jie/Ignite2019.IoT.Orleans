using System;
using Ignite2019.IoT.Orleans.Model;

namespace Ignite2019.IoT.Orleans.Events
{
    public abstract class DeviceEvent:ICloneable
    {
        public string DeviceId { get; set; }

        public DateTime EventTime => DateTime.Now;

        public EventHistory ToEventHistory()
        {
            return new EventHistory(this.DeviceId, this.EventTime);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}