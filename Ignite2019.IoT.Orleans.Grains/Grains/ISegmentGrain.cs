using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.Model;
using Orleans;

namespace Ignite2019.IoT.Orleans.Grains
{
    public interface ISegmentGrain :IGrainWithGuidKey
    {

        /// <summary>
        /// 获取段号（必须单例调用）
        /// </summary>
        /// <returns></returns>
        Task<Segment> GeSegmentAsync(int productId);

    }
}