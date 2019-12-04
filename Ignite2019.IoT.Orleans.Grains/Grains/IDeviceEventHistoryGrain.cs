using System.Threading.Tasks;
using Orleans;

namespace Ignite2019.IoT.Orleans.Grains
{
    public interface IDeviceEventHistoryGrain : IGrainWithGuidKey
    {
        Task GetEventHistories();
    }
}