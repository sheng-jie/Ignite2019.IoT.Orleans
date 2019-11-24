using System.Threading.Tasks;
using Orleans;

namespace Ignite2019.IoT.Orleans.Grains
{
    public interface ISegmentGrain : IGrainWithIntegerKey
    {
        /// <summary>
        /// 在当前段号下申请新的的设备ID
        /// </summary>
        /// <returns></returns>
        Task<string> NewDeviceIdAsync();
    }
}