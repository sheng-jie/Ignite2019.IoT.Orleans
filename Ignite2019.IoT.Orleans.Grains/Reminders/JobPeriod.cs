using System;

namespace Ignite2019.IoT.Orleans.Reminders
{
    public class JobPeriod
    {
        public DateTime StartTime { get; set; }

        public long Period { get; set; }

        public DateTime EndTime { get; set; }

        public JobPeriod(DateTime startTime, long period, DateTime endTime)
        {
            StartTime = startTime;
            Period = period;
            EndTime = endTime;
        }

    }
}