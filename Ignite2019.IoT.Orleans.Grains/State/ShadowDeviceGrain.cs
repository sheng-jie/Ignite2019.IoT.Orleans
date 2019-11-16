using System.Linq;
using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.Model;
using Orleans;
using WalkingTec.Mvvm.Core;

namespace Ignite2019.IoT.Orleans.Grains.State
{
    public class ShadowDeviceGrain : Grain<ShadowDevice>, IShadowDeviceGrain
    {
        public string DeviceId => this.GetPrimaryKeyString();
        private readonly IDataContext _dataContext;

        public ShadowDeviceGrain(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public override async Task OnActivateAsync()
        {
            await LoadShadowDevice();
        }

        private async Task LoadShadowDevice()
        {
            await this.ReadStateAsync();
            if (string.IsNullOrEmpty(this.State.Device?.ID))
            {
                var device = _dataContext.Set<Device>().FirstOrDefault(d => d.ID == this.DeviceId);
                this.State.Device = device;

                var eventHistories = _dataContext.Set<EventHistory>().Where(eh => eh.DeviceId == this.DeviceId);
                this.State.EventHistories = eventHistories.ToList();
                await this.WriteStateAsync();
            }
        }


        public override async Task OnDeactivateAsync()
        {
            await this.WriteStateAsync();

            var newAddedHistories = this.State.EventHistories.Where(eh => eh.ID == 0);

            await this._dataContext.Set<EventHistory>().AddRangeAsync(newAddedHistories);
        }

        public Task<ShadowDevice> GetShadowDevice()
        {
            return Task.FromResult(this.State);
        }

        public Task AddEventHistory(EventHistory newHistory)
        {
            this.State.EventHistories.Add(newHistory);
            return Task.CompletedTask;
        }

        public Task UpdateStatus(bool isOnline)
        {
            this.State.IsOnline = isOnline;
            return Task.CompletedTask;
        }
    }
}