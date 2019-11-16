using System;

namespace Ignite2019.IoT.Orleans.Model
{
    public class DeviceBind
    {
        public int UserId { get; set; }

        public Guid DeviceId { get; set; }

        public bool IsAdmin { get; set; }

        public DateTime BindTime { get; set; }

        public DeviceBindStatus Status { get; set; }
    }
}