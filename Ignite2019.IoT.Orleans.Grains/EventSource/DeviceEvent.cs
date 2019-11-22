using System;

namespace Ignite2019.IoT.Orleans.Grains.EventSource
{
    public abstract class DeviceEvent
    {
        public string DeviceId { get; set; }

        public DateTime EventTime { get; set; }
    }
}