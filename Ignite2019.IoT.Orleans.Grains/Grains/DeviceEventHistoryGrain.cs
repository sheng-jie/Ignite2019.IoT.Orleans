using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.DataAccess;
using Ignite2019.IoT.Orleans.Events;
using Ignite2019.IoT.Orleans.Model;
using Orleans;
using Orleans.Streams;
using WalkingTec.Mvvm.Core;

namespace Ignite2019.IoT.Orleans.Grains
{
    public class DeviceEventHistoryGrain : Grain, IDeviceEventHistoryGrain
    {
        public DataContext DataContext { get; set; }

        private IAsyncStream<DeviceEvent> _deviceEventStream;

        private List<EventHistory> _eventHistories = new List<EventHistory>();

        public DeviceEventHistoryGrain()
        {
            this.DataContext = new DataContext("Server=(localdb)\\mssqllocaldb;Database=Orleans_db;Trusted_Connection=True;MultipleActiveResultSets=true", DBTypeEnum.SqlServer);
        }

        public override Task OnActivateAsync()
        {
            var streamProvider = this.GetStreamProvider("SMSProvider");
            var primaryKey = this.GetPrimaryKey();
            _deviceEventStream = streamProvider.GetStream<DeviceEvent>(primaryKey, "DeviceEvent");
            _deviceEventStream.SubscribeAsync(async (deviceEvent, token) => await this.AddEventHistory(deviceEvent));
            return base.OnActivateAsync();
        }

        private async Task AddEventHistory(DeviceEvent deviceEvent)
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

            await this.DataContext.EventHistories.AddAsync(eventHistory);
            await this.DataContext.SaveChangesAsync();

            Console.WriteLine($"Added new event history for device Id {eventHistory.DeviceId} :{eventHistory.EventType}");

        }

        public Task GetEventHistories()
        {
            return Task.FromResult(_eventHistories);
        }
    }
}
