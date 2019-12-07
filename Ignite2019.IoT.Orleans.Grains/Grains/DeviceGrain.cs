using System;
using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.Events;
using Ignite2019.IoT.Orleans.States;
using Orleans;
using Orleans.EventSourcing;
using Orleans.Providers;
using Orleans.Streams;

namespace Ignite2019.IoT.Orleans.Grains
{
    public class DeviceGrain : JournaledGrain<ShadowDevice, DeviceEvent>, IDeviceGrain
    {
        private IAsyncStream<DeviceEvent> _deviceEventStream;

        public override Task OnActivateAsync()
        {
            var streamProvider = this.GetStreamProvider("SMSProvider");
            var streamId = Guid.NewGuid();
            _deviceEventStream = streamProvider.GetStream<DeviceEvent>(streamId, "DeviceEvent");
            return base.OnActivateAsync();
        }

        public Task HandleEvent(DeviceEvent deviceEvent)
        {
            switch (deviceEvent)
            {
                case OnlineEvent newEvent:
                    this.RaiseEvent(newEvent);
                    break;
                case OfflineEvent newEvent:
                    this.RaiseEvent(newEvent);
                    break;
                case ReportEvent newEvent:
                    this.RaiseEvent(newEvent);
                    break;
                case ControlEvent newEvent:
                    this.RaiseEvent(newEvent);
                    break;

            }

            ConfirmEvents();

            _deviceEventStream.OnNextAsync(deviceEvent);

            return Task.CompletedTask;

        }



        public async Task<ShadowDevice> GetShadowDeviceAsync()
        {
            await RefreshNow();
            return this.State;
        }
    }
}