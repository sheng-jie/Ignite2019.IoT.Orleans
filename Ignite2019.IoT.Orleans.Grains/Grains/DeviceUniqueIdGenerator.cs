using System.Threading.Tasks;
using Orleans;

namespace Ignite2019.IoT.Orleans.Grains
{
    public class DeviceUniqueIdGenerator : Grain, IUniqueIdGenerator
    {
        public async Task<string> NewId()
        {
            var productId = (int)this.GetPrimaryKeyLong();
            var segmentGrain = this.GrainFactory.GetGrain<ISegmentGrain>(productId);

            var newDeviceId = await segmentGrain.NewDeviceIdAsync();

            return newDeviceId;

        }
    }
}