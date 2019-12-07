using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WalkingTec.Mvvm.Core;

namespace Ignite2019.IoT.Orleans.Model
{
    public class BackgroundJob : TopBasePoco
    {
        [NotMapped]
        public bool Repeatable => !EndTime.HasValue;

        [Display(Name = "设备Id")]
        public string DeviceId { get; set; }

        [Display(Name = "任务状态")]
        public JobStatus JobStatus { get; set; }

        [Display(Name = "定时任务")]
        public string Command { get; set; }

        [Display(Name = "开始时间")]
        public DateTime StartTime { get; set; }

        [Display(Name = "执行间隔")]
        public TimeSpan Period { get; set; }

        [Display(Name = "结束时间")]
        public DateTime? EndTime { get; set; }


        [Display(Name = "执行次数")]
        public int ExecutedCount { get; set; }

        [Display(Name = "上次执行时间")]
        public DateTime? LastExecuteTime { get; set; }

        [NotMapped]
        public bool IsStopped => !this.Repeatable && this.ExecutedCount > 0
                                 || this.EndTime < DateTime.Now
                                 || this.JobStatus == JobStatus.Stopped;
    }
}