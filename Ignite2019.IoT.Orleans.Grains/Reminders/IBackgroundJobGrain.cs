using System.Threading.Tasks;
using Orleans;

namespace Ignite2019.IoT.Orleans.Reminders
{
    /// <summary>
    ///后台定时任务(组合key：Guid + DeviceId)
    /// </summary>
    public interface IBackgroundJobGrain : IGrainWithGuidCompoundKey
    {
        /// <summary>
        /// create short period timer between 1 mins and 49 days
        /// </summary>
        /// <param name="command"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        Task CreateReminder(string command, JobPeriod period);

        /// <summary>
        /// create short period timer less than 1 mins
        /// </summary>
        /// <param name="command"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        Task CreateTimer(string command, JobPeriod period);
    }
}
