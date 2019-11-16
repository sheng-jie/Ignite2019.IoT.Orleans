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

        [Display(Name = "设备")]
        public string DeviceId { get; set; }
        [Display(Name = "设备")]
        public Device Device { get; set; }

        [Display(Name = "事件类型")]
        public EventType EventType { get; set; }
        [Display(Name = "详情")]
        public string Detail { get; set; }

        [Display(Name = "触发日期")]
        public DateTime UpdateTime { get; set; }

        [Display(Name = "操作用户")]
        public int? UserId { get; set; }
        [Display(Name = "操作用户")]
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