using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.Events;
using Ignite2019.IoT.Orleans.Model;
using Orleans;
using Orleans.Streams;

namespace Ignite2019.IoT.Orleans.Grains
{
    [ImplicitStreamSubscription("DeviceEvent")]
    public class DeviceEventHistoryGrain : Grain, IDeviceEventHistoryGrain
    {
        private IAsyncStream<DeviceEvent> _deviceEventStream;

        private List<EventHistory> _eventHistories = new List<EventHistory>();

        public override Task OnActivateAsync()
        {
            var streamProvider = this.GetStreamProvider("SMSProvider");

            _deviceEventStream = streamProvider.GetStream<DeviceEvent>(Guid.Empty, "DeviceEvent");
            _deviceEventStream.SubscribeAsync(async (deviceEvent, token) => await this.AddEventHistory(deviceEvent));
            return base.OnActivateAsync();
        }

        private Task AddEventHistory(DeviceEvent deviceEvent)
        {
            var eventHistory = deviceEvent.ToEventHistory();
            switch (deviceEvent)
            {
                case ReportEvent newEvent:
                    eventHistory.Detail = newEvent.Data;
                    break;
                case ControlEvent newEvent:
                    eventHistory.Detail = newEvent.Command.CommandBody;
                    break;

            }
            this._eventHistories.Add(eventHistory);

            Console.WriteLine($"Added new event for device Id {eventHistory.DeviceId} :{eventHistory.EventType}");
            return Task.CompletedTask;
        }

        public Task GetEventHistories()
        {
            return Task.FromResult(_eventHistories);
        }
    }
}
