using System;
using System.Threading.Tasks;
using Orleans;

namespace Ignite2019.IoT.Orleans.Model.Grains
{
    /// <summary>
    /// 唯一Id生成器
    /// </summary>
    public interface IUniqueIdGenerator:IGrainWithStringKey
    {
        Task<string> NewId();
    }

    public class GuidGenerator:Grain,IUniqueIdGenerator
    {
        public Task<string> NewId()
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }
    }

    public class DeviceUniqueIdGenerator:Grain<Segment>,IUniqueIdGenerator
    {
        public Task<string> NewId()
        {
            var companyId = this.GetPrimaryKey();
            // get assigned segments of company.
            

            var newId = Guid.NewGuid().ToString();

            return Task.FromResult(newId);
        }
    }

    public class Segment
    {
        public ulong LatestNum { get; set; }
    }
}