using System;
using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.Events;
using Ignite2019.IoT.Orleans.Model;
using Ignite2019.IoT.Orleans.States;
using Orleans;
using Orleans.EventSourcing;

namespace Ignite2019.IoT.Orleans.Grains
{
    public class DeviceGrain : JournaledGrain<ShadowDevice>, IDeviceGrain
    {
        public Task HandleEvent(DeviceEvent deviceEvent)
        {
            var evnt = deviceEvent as OnlineEvent;
            this.RaiseEvent(evnt);

            return ConfirmEvents();
        }
    }
}