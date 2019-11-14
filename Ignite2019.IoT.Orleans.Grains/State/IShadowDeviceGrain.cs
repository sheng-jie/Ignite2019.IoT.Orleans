using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.Model;
using Orleans;
using Orleans.Runtime;


namespace Ignite2019.IoT.Orleans.Grains.State
{
    public interface IShadowDeviceGrain:IGrainWithStringKey
    {
        Task<Device> GetShadowDevice();
    }

    public class ShadowDeviceGrain:Grain,IShadowDeviceGrain
    {
        public ShadowDeviceGrain(IPersistentState<ShadowDevice> device)
        {
            
        }
        public Task<Device> GetShadowDevice()
        {
            throw new System.NotImplementedException();
        }
    }

    public class ShadowDevice
    {

    }
}