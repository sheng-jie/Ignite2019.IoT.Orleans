using System.Collections.Generic;
using Ignite2019.IoT.Orleans.Events;
using Ignite2019.IoT.Orleans.Model;

namespace Ignite2019.IoT.Orleans.States
{
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
            eventHistory.EventType = EventType.Online;
            this.IsOnline = true;
            this.AddEventHistory(eventHistory);

            return this;
        }

        public ShadowDevice Apply(OfflineEvent offlineEvent)
        {
            var eventHistory = offlineEvent.ToEventHistory();
            eventHistory.EventType = EventType.Offline;
            this.IsOnline = false;
            this.AddEventHistory(eventHistory);
            return this;
        }

        public ShadowDevice Apply(ReportEvent reportEvent)
        {
            var eventHistory = reportEvent.ToEventHistory();
            eventHistory.EventType = EventType.Report;
            eventHistory.Detail = reportEvent.Data;
            this.AddEventHistory(eventHistory);

            return this;
        }

        public ShadowDevice Apply(ControlEvent controlEvent)
        {

            var eventHistory = controlEvent.ToEventHistory();
            eventHistory.EventType = EventType.Control;
            eventHistory.Detail = controlEvent.Command;
            eventHistory.UserId = controlEvent.UserId;
            this.AddEventHistory(eventHistory);

            return this;
        }

        private void AddEventHistory(EventHistory eventHistory) =>
            this.EventHistories.Add(eventHistory);
    }

}