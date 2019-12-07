using System;
using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.DataAccess;
using Ignite2019.IoT.Orleans.Events;
using Ignite2019.IoT.Orleans.Grains;
using Ignite2019.IoT.Orleans.Model;
using Microsoft.EntityFrameworkCore;
using Orleans;
using Orleans.Runtime;
using WalkingTec.Mvvm.Core;

namespace Ignite2019.IoT.Orleans.Reminders
{
    public class BackgroundJobGrain : Grain<BackgroundJob>, IBackgroundJobGrain, IRemindable
    {
        private IDisposable _timer;

        public DataContext DataContext { get; set; }

        public BackgroundJobGrain()
        {
            this.DataContext = new DataContext("Server=(localdb)\\mssqllocaldb;Database=Orleans_db;Trusted_Connection=True;MultipleActiveResultSets=true", DBTypeEnum.SqlServer);
        }


        protected override async Task WriteStateAsync()
        {
            await base.WriteStateAsync();

            await this.DataContext.SaveChangesAsync();
        }

        private async Task AddNewBackgroundJob(string command, JobPeriod period, BackgroundJobType type)
        {
            this.GetPrimaryKey(out var deviceId);

            var backgroundJob = new BackgroundJob()
            {
                Command = command,
                JobType = type,
                DeviceId = deviceId,
                StartTime = period.StartTime,
                EndTime = period.EndTime,
                Period = period.Period,
                JobStatus = JobStatus.Pending
            };

            this.State = backgroundJob;

            await this.DataContext.AddAsync(this.State);

            await this.WriteStateAsync();

        }

        public async Task CreateReminder(string command, JobPeriod period)
        {
            await AddNewBackgroundJob(command, period, BackgroundJobType.Reminder);

            var primaryKey = this.GetPrimaryKey(out var deviceId);

            await this.RegisterOrUpdateReminder(primaryKey.ToString(), TimeSpan.FromMinutes(period.Period),
                TimeSpan.FromMinutes(period.Period));

        }

        public async Task CreateTimer(string command, JobPeriod period)
        {
            await AddNewBackgroundJob(command, period, BackgroundJobType.Timer);

            _timer = this.RegisterTimer(async (state) =>
            {
                if (DateTime.Now > this.State.StartTime)
                {
                    if (this.State.IsStopped)
                    {
                        this._timer.Dispose();
                        this.State.JobStatus = JobStatus.Stopped;
                        await this.WriteStateAsync();
                        return;
                    }

                    await HandleBackgroundJob();
                }

            }, this.State, TimeSpan.FromSeconds(period.Period), TimeSpan.FromSeconds(period.Period));

        }


        public async Task ReceiveReminder(string reminderName, TickStatus status)
        {
            if (DateTime.Now > this.State.StartTime)
            {
                if (this.State.IsStopped)
                {
                    var reminder = await this.GetReminder(reminderName);
                    await UnregisterReminder(reminder);
                    this.State.JobStatus = JobStatus.Stopped;

                    await this.WriteStateAsync();
                    return;
                }

                await HandleBackgroundJob();
            }
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

            this.DataContext.Update(this.State);
            await this.WriteStateAsync();

        }
    }
}