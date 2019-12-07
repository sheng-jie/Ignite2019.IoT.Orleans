using System;

namespace Ignite2019.IoT.Orleans.Reminders
{
    public class JobPeriod
    {
        public bool Repeatable => !EndTime.HasValue;

        public TimeSpan DueTime { get; }

        public DateTime StartTime { get; set; }

        public TimeSpan Period { get; set; }

        public DateTime? EndTime { get; set; }

        public JobPeriod(DateTime startTime, TimeSpan period, DateTime? endTime)
        {
            StartTime = startTime;
            Period = period;
            EndTime = endTime;
            DueTime = this.StartTime - DateTime.Now;
        }

    }
}