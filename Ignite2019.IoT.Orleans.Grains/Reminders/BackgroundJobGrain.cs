using System;
using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.Events;
using Ignite2019.IoT.Orleans.Grains;
using Ignite2019.IoT.Orleans.Model;
using Orleans;
using Orleans.Runtime;

namespace Ignite2019.IoT.Orleans.Reminders
{
    public class BackgroundJobGrain : Grain<BackgroundJob>, IBackgroundJobGrain, IRemindable
    {
        private IDisposable _timer;
        private IGrainReminder _remindable;

        private async Task AddNewBackgroundJob(string command, JobPeriod period)
        {
            this.GetPrimaryKey(out var deviceId);

            var backgroundJob = new BackgroundJob()
            {
                Command = command,
                DeviceId = deviceId,
                StartTime = period.StartTime,
                EndTime = period.EndTime,
                Period = period.Period,
                JobStatus = JobStatus.Pending
            };

            this.State = backgroundJob;
            await this.WriteStateAsync();
        }

        public async Task CreateReminder(string command, JobPeriod period)
        {
            var primaryKey = this.GetPrimaryKey();

            _remindable = await this.RegisterOrUpdateReminder(primaryKey.ToString(), period.DueTime, period.Period);

            await AddNewBackgroundJob(command, period);
        }

        public async Task CreateTimer(string command, JobPeriod period)
        {
            await AddNewBackgroundJob(command, period);

            _timer = this.RegisterTimer(async (state) =>
            {
                if (this.State.IsStopped)
                {
                    this._timer.Dispose();
                    this.State.JobStatus = JobStatus.Stopped;
                    await this.WriteStateAsync();
                    return;
                }

                await HandleBackgroundJob();

            }, this.State, period.DueTime, period.Period);

        }


        public async Task ReceiveReminder(string reminderName, TickStatus status)
        {
            if (this.State.IsStopped)
            {
                await UnregisterReminder(_remindable);
                this._remindable = null;
                return;
            }

            await HandleBackgroundJob();
        }

        private async Task HandleBackgroundJob()
        {
            this.GetPrimaryKey(out var deviceId);
            var control = new ControlCommand(this.State.Command);

            var controlEvent = new ControlEvent(control)
            {
                DeviceId = deviceId
            };

            var grain = this.GrainFactory.GetGrain<IDeviceGrain>(controlEvent.DeviceId);

            await grain.HandleEvent(controlEvent);

            this.State.ExecutedCount++;
            this.State.JobStatus = JobStatus.Executed;
            this.State.LastExecuteTime = DateTime.Now;

            await this.WriteStateAsync();
        }
    }
}