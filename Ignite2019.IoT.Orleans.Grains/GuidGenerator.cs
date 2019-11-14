using System;
using System.Threading.Tasks;
using Orleans;

namespace Ignite2019.IoT.Orleans.Grains
{
    public class GuidGenerator : Grain, IUniqueIdGenerator
    {
        public Task<string> NewId()
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }
    }
}