using System;
using System.Collections.Generic;
using Ignite2019.IoT.Orleans.Events;
using Ignite2019.IoT.Orleans.Model;

namespace Ignite2019.IoT.Orleans.States
{
    [Serializable]
    public class ShadowDevice
    {
        public ShadowDevice()
        {
            this.EventHistories = new List<EventHistory>();
        }

        public Device Device { get; set; }

        public List<EventHistory> EventHistories { get; set; }

        public bool IsOnline { get; set; }

        public ShadowDevice Apply(OnlineEvent onlineEvent)
        {
            var eventHistory = onlineEvent.ToEventHistory();
            this.IsOnline = true;
            this.AddEventHistory(eventHistory);

            Console.WriteLine($"{onlineEvent.DeviceId} is online at {onlineEvent.EventTime}");

            return this;
        }

        public ShadowDevice Apply(OfflineEvent offlineEvent)
        {
            var eventHistory = offlineEvent.ToEventHistory();
            this.IsOnline = false;
            this.AddEventHistory(eventHistory);

            Console.WriteLine($"{offlineEvent.DeviceId} is offline at {offlineEvent.EventTime}");

            return this;
        }

        public ShadowDevice Apply(ReportEvent reportEvent)
        {
            var eventHistory = reportEvent.ToEventHistory();
            eventHistory.Detail = reportEvent.Data;
            this.AddEventHistory(eventHistory);

            Console.WriteLine($"{reportEvent.DeviceId} reports data {reportEvent.Data} at {reportEvent.EventTime}");

            return this;
        }

        public ShadowDevice Apply(ControlEvent controlEvent)
        {
            var eventHistory = controlEvent.ToEventHistory();
            eventHistory.Detail = controlEvent.Command.CommandBody;
            eventHistory.UserId = controlEvent.UserId;

            this.AddEventHistory(eventHistory);

            Console.WriteLine($"{controlEvent.DeviceId} sends a [{controlEvent.Command.CommandBody}] command  at {controlEvent.EventTime}");

            return this;
        }

        private void AddEventHistory(EventHistory eventHistory) =>
            this.EventHistories.Add(eventHistory);
    }

}
