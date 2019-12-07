using System;

namespace Ignite2019.IoT.Orleans.Model
{
    public class BackgroundJob
    {
        public bool Repeatable => !EndTime.HasValue;

        public string DeviceId { get; set; }

        public JobStatus JobStatus { get; set; }

        public string Command { get; set; }

        public DateTime StartTime { get; set; }

        public TimeSpan Period { get; set; }

        public DateTime? EndTime { get; set; }

        public int ExecutedCount { get; set; }

        public DateTime? LastExecuteTime { get; set; }

        public bool IsStopped => !this.Repeatable && this.ExecutedCount > 0
                                 || this.EndTime < DateTime.Now
                                 || this.JobStatus == JobStatus.Stopped;
    }
}