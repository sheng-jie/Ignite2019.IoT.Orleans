using System.Threading.Tasks;
using Orleans;

namespace Ignite2019.IoT.Orleans.Grains
{
    /// <summary>
    /// 唯一Id生成器
    /// </summary>
    public interface IUniqueIdGenerator : IGrainWithIntegerKey
    {
        Task<string> NewId();
    }
}