using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WalkingTec.Mvvm.Core;

namespace Ignite2019.IoT.Orleans.Model
{

    public enum BackgroundJobType
    {
        [Display(Name = "秒表")]
        Timer,
        [Display(Name = "闹钟")]
        Reminder,
    }
    public class BackgroundJob : TopBasePoco
    {
        private DateTime? _startTime;
        private DateTime? _endTime;

        [Display(Name = "设备Id")]
        [Required]
        public string DeviceId { get; set; }

        [Display(Name = "任务类型")]
        [Required]
        public BackgroundJobType JobType { get; set; }

        [Display(Name = "任务状态")]
        public JobStatus JobStatus { get; set; }

        [Display(Name = "定时任务")]
        [Required]
        public string Command { get; set; }

        [Display(Name = "开始时间")]
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartTime
        {
            get => _startTime.HasValue ? _startTime.Value : DateTime.Now;
            set => _startTime = value;
        }

        [Display(Name = "执行间隔")]
        [DefaultValue(1)]
        public long Period { get; set; }

        [Display(Name = "结束时间")]
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? EndTime
        {
            get => _endTime.HasValue ? _endTime.Value : DateTime.Now.AddMinutes(3);
            set => _endTime = value;
        }


        [Display(Name = "执行次数")]
        public int ExecutedCount { get; set; }

        [Display(Name = "上次执行时间")]
        public DateTime? LastExecuteTime { get; set; }

        [NotMapped]
        public bool IsStopped => this.EndTime < DateTime.Now
                                 || this.JobStatus == JobStatus.Stopped;
    }
}