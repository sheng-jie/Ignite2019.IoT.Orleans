using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.Model;
using Orleans;

namespace Ignite2019.IoT.Orleans.Grains
{
    public interface ISegmentGrain : IGrainWithGuidKey
    {
        Task<Segment> GetAvailableSegmentAsync(int productId);
    }
}