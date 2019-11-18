using System;
using System.Linq;
using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.DataAccess;
using Ignite2019.IoT.Orleans.Grains.State;
using Ignite2019.IoT.Orleans.Model;
using Microsoft.EntityFrameworkCore;
using Orleans;
using WalkingTec.Mvvm.Core;

namespace Ignite2019.IoT.Orleans.Grains
{
    public class DeviceUniqueIdGenerator : Grain<Segment>, IUniqueIdGenerator
    {
        public DataContext DataContext { get; set; }
        public DeviceUniqueIdGenerator(DbContext context)
        {
            //this.DataContext = context;
            this.DataContext = new DataContext("Server=(localdb)\\mssqllocaldb;Database=Orleans_db;Trusted_Connection=True;MultipleActiveResultSets=true", DBTypeEnum.SqlServer);
        }

        public override async Task OnActivateAsync()
        {
            await base.OnDeactivateAsync();
            await LoadSegment();
            // save state every minutes
            this.RegisterTimer(async o =>
            {
                await this.WriteStateAsync();
                //this.DataContext.Set<Segment>().Attach(this.State);

                await this.DataContext.SaveChangesAsync();
            }, null, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(1));
        }

        public async Task<string> NewId()
        {
            await LoadSegment();
            ulong newNum = 0;

            if (this.State.HasRemain)
            {
                newNum = this.State.MaxNum - this.State.Remain;
                this.State.Remain--;
            }

            var newId = $"{newNum:X16}";

            return newId;
        }

        private async Task LoadSegment()
        {
            // get assigned segments of product.
            if (!this.State.HasRemain)
            {
                var productId = this.GetPrimaryKeyLong();

                var segmentGrain = this.GrainFactory.GetGrain<ISegmentGrain>(Guid.Empty);

                var segment = await segmentGrain.GetAvailableSegmentAsync((int)productId);

                this.State = segment;
                await this.WriteStateAsync();
            }
        }

        public override async Task OnDeactivateAsync()
        {
            await this.WriteStateAsync();
            //this.DataContext.Set<Segment>().Attach(this.State);

            await this.DataContext.SaveChangesAsync();

            await this.DataContext.SaveChangesAsync();
            await base.OnDeactivateAsync();
        }
    }
}