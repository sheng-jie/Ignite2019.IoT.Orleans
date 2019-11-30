using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.Events;
using Ignite2019.IoT.Orleans.States;
using Orleans.EventSourcing;
using Orleans.Providers;

namespace Ignite2019.IoT.Orleans.Grains
{
    public class DeviceGrain : JournaledGrain<ShadowDevice,DeviceEvent>, IDeviceGrain
    {
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

            return Task.CompletedTask;
            //return ConfirmEvents();
        }



        public async Task<ShadowDevice> GetShadowDeviceAsync()
        {
            await RefreshNow();
            return this.State;
        }
    }
}