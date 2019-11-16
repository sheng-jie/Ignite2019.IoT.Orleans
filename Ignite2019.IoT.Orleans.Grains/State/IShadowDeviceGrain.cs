using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.Model;
using Orleans;


namespace Ignite2019.IoT.Orleans.Grains.State
{
    public interface IShadowDeviceGrain : IGrainWithStringKey
    {
        Task<ShadowDevice> GetShadowDevice();

        Task AddEventHistory(EventHistory newHistory);

        Task UpdateStatus(bool isOnline);
    }
}