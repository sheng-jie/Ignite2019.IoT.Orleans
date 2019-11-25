using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.Events;
using Orleans;

namespace Ignite2019.IoT.Orleans.Grains
{
    public interface IDeviceGrain : IGrainWithStringKey
    {
        Task<States.ShadowDevice> GetShadowDeviceAsync();
        Task HandleEvent(DeviceEvent deviceEvent);
    }
}