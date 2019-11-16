using System;

namespace Ignite2019.IoT.Orleans.Model
{
    public class DeviceRequest
    {
        public string DeviceId { get; set; }

        public string Body { get; set; }

        public EventType EventType { get; set; }

        public DateTime RequestTime { get; set; }
    }
}