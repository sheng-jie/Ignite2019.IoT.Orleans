using System.Threading.Tasks;
using Orleans;

namespace Ignite2019.IoT.Orleans.Grains.EventSource
{
    internal interface IDeviceGrain : IGrainWithStringKey
    {
        Task Online();
        Task Offline();
        Task Report();
        Task Control();
    }
}