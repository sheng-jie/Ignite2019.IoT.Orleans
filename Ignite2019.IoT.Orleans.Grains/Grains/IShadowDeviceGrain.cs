using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.States;
using Orleans;

namespace Ignite2019.IoT.Orleans.Grains
{
    public interface IShadowDeviceGrain : IGrainWithStringKey
    {
        Task<ShadowDevice> GetShadowDevice();
    }
}