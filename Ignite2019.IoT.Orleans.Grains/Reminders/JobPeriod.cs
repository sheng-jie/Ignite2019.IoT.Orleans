using System;

namespace Ignite2019.IoT.Orleans.Reminders
{
    public class JobPeriod
    {
        public bool Repeatable => !EndTime.HasValue;

        public TimeSpan DueTime { get; }

        public DateTime StartTime { get; set; }

        public long Period { get; set; }

        public DateTime? EndTime { get; set; }

        public JobPeriod(DateTime startTime, long period, DateTime? endTime)
        {
            StartTime = startTime;
            Period = period;
            EndTime = endTime;
            if (this.StartTime>DateTime.Now)
            {
                var timespan = this.StartTime.TimeOfDay - DateTime.Now.TimeOfDay;
                DueTime = timespan;
            }
            else
            {
                DueTime = DateTime.Now.AddSeconds(3).TimeOfDay;
            }
        }

    }
}