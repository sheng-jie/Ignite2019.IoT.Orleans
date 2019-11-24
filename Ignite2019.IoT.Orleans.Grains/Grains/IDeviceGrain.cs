using System;
using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.Events;
using Ignite2019.IoT.Orleans.States;
using Orleans;
using Orleans.EventSourcing;

namespace Ignite2019.IoT.Orleans.Grains
{
    internal interface IDeviceGrain : IGrainWithStringKey
    {
        Task Online();
        Task Offline();
        Task Report(string data);
        Task Control(string controlCommand);
    }

    internal class DeviceGrain:JournaledGrain<ShadowDevice,DeviceEvent>,IDeviceGrain
    {
        private string DeviceId => this.GetPrimaryKeyString();
        public Task Online()
        {
            this.RaiseEvent(new OnlineEvent()
            {
                DeviceId = DeviceId,
                EventTime = DateTime.Now
            });

            return ConfirmEvents();
        }

        public Task Offline()
        {
            this.RaiseEvent(new OfflineEvent()
            {
                DeviceId = DeviceId,
                EventTime = DateTime.Now
            });

            return ConfirmEvents();
        }

        public Task Report(string data)
        {
            this.RaiseEvent(new ReportEvent()
            {
                DeviceId = DeviceId,
                Data = data,
                EventTime = DateTime.Now
            });

            return ConfirmEvents();
        }

        public Task Control(string controlCommand)
        {
            this.RaiseEvent(new ControlEvent()
            {
                DeviceId = DeviceId,
                Command = controlCommand,
                EventTime = DateTime.Now
            });

            return ConfirmEvents();
        }
    }
}