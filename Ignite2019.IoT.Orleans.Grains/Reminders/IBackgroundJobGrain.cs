using System.Threading.Tasks;
using Orleans;

namespace Ignite2019.IoT.Orleans.Reminders
{
    /// <summary>
    ///后台定时任务(组合key：Guid + DeviceId)
    /// </summary>
    public interface IBackgroundJobGrain : IGrainWithGuidCompoundKey
    {
        Task CreateReminder(string command, JobPeriod period);

        Task CreateTimer(string command, JobPeriod period);
    }
}
