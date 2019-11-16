using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WalkingTec.Mvvm.Core;

namespace Ignite2019.IoT.Orleans.Model
{
    public class EventHistory : TopBasePoco
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new long ID { get; set; }

        public string DeviceId { get; set; }
        public Device Device { get; set; }

        public EventType EventType { get; set; }
        public string Detail { get; set; }

        public DateTime UpdateTime { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

        public EventHistory(string deviceId, EventType eventType)
        {
            this.EventType = eventType;
            this.UpdateTime = DateTime.Now;
            this.DeviceId = deviceId;
        }

        public EventHistory()
        {
        }
    }
}