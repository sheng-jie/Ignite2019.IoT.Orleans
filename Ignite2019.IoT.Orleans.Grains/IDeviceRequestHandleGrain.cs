using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.Grains.State;
using Ignite2019.IoT.Orleans.Model;
using Orleans;

namespace Ignite2019.IoT.Orleans.Grains
{
    public interface IDeviceRequestHandleGrain : IGrainWithStringKey
    {
        Task<bool> Handle(DeviceRequest request);
    }

    public class DeviceRequestHandleGrain : Grain, IDeviceRequestHandleGrain
    {
        public string DeviceId => this.GetPrimaryKeyString();

        public Task<bool> Handle(DeviceRequest request)
        {
            
            var shadowDeviceGrain = this.GrainFactory.GetGrain<IShadowDeviceGrain>(this.DeviceId);
            if (request.EventType==EventType.Online||request.EventType == EventType.Offline)
            {
                shadowDeviceGrain.UpdateStatus(request.EventType == EventType.Online);
            }

            if (request.EventType == EventType.UnlockRequest)
            {
                //Send unlock request to bind user
            }

            shadowDeviceGrain.AddEventHistory(new EventHistory(request.DeviceId, request.EventType)
            {
                UpdateTime = request.RequestTime,
                Detail = request.Body
            });
            
            //发布状态更新消息，由shadow device 订阅，并保存状态更新

            return Task.FromResult(true);

        }
    }
}