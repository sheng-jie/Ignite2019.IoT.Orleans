using System;
using System.Linq;
using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.Events;
using Ignite2019.IoT.Orleans.Model;
using Ignite2019.IoT.Orleans.States;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.Extensions.Localization.Internal;
using Orleans;
using Orleans.EventSourcing;
using Orleans.Runtime;
using WalkingTec.Mvvm.Core;

namespace Ignite2019.IoT.Orleans.Grains
{
    public class ShadowDeviceGrain : JournaledGrain<ShadowDevice, DeviceEvent>, IShadowDeviceGrain
    {
        private IDisposable _timer;

        public Task<ShadowDevice> GetShadowDevice()
        {
            return Task.FromResult(this.State);
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

            return ConfirmEvents();
        }

        /// <summary>
        /// connect to the device
        /// </summary>
        /// <returns></returns>
        public async Task Login()
        {
            if (this.State.IsOnline)
            {
                await this.SendHeartbeat();
                //注册定时器，1分钟后登出
                _timer = this.RegisterTimer(async (state) =>
                {
                    if (this.State.IsOnline)
                    {
                        await this.Logout();
                    }
                }, this.State, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            }
        }

        /// <summary>
        /// disconnect from the device
        /// </summary>
        /// <returns></returns>
        public async Task Logout()
        {
            if (this.State.IsOnline)
            {
                var offlineEvent = new OfflineEvent()
                {
                    DeviceId = this.State.Device.ID
                };
                this.RaiseEvent(offlineEvent);
                await this.ConfirmEvents();

                this._timer.Dispose();// unregistered the timer;
                this.State.IsOnline = false;
            }
        }

        /// <summary>
        /// send heartbeat to keep device alive
        /// </summary>
        /// <returns></returns>
        private Task SendHeartbeat()
        {
            this.State.IsOnline = true;
            return Task.CompletedTask;
        }
    }
}