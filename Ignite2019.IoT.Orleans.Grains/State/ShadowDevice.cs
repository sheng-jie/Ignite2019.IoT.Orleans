using System.Collections.Generic;
using Ignite2019.IoT.Orleans.Model;

namespace Ignite2019.IoT.Orleans.Grains.State
{
    public class ShadowDevice
    {
        public ShadowDevice()
        {
            this.EventHistories = new List<EventHistory>();
        }
        public Device Device { get; set; }
        public List<EventHistory> EventHistories { get; set; }
        public bool IsOnline { get; set; }

    }
}