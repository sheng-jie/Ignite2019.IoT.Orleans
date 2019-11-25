using System;
using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.DataAccess;
using Ignite2019.IoT.Orleans.Model;
using Orleans;
using WalkingTec.Mvvm.Core;

namespace Ignite2019.IoT.Orleans.Grains
{
    public class DeviceUniqueIdGenerator : Grain<Segment>, IUniqueIdGenerator
    {
        public DataContext DataContext { get; set; }

        public int ProductId => (int)this.GetPrimaryKeyLong();
        public DeviceUniqueIdGenerator()
        {
            this.DataContext = new DataContext("Server=(localdb)\\mssqllocaldb;Database=Orleans_db;Trusted_Connection=True;MultipleActiveResultSets=true", DBTypeEnum.SqlServer);
        }

        public override async Task OnActivateAsync()
        {
            await this.ReadStateAsync();
            await LoadSegmentAsync();

            // save state every minutes
            this.RegisterTimer(async o =>
            {
                await this.WriteStateAsync();

            }, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1));
        }

        public async Task<string> NewId()
        {
            await LoadSegmentAsync();

            var newNum = this.State.MaxNum - this.State.Remain;
            this.State.Remain--;

            return $"{newNum:X16}";

        }

        protected override async Task WriteStateAsync()
        {
            await base.WriteStateAsync();

            this.DataContext.Attach(this.State);
            var result = await this.DataContext.SaveChangesAsync();
        }

        public override async Task OnDeactivateAsync()
        {
            await this.WriteStateAsync();
            await base.OnDeactivateAsync();
        }

        /// <summary>
        /// 加载Segment
        /// </summary>
        /// <returns></returns>
        private async Task LoadSegmentAsync()
        {
            if (this.State.HasRemain)
            {
                return;
            }

            //单例创建，避免冲突
            var segmentGrain = this.GrainFactory.GetGrain<ISegmentGrain>(Guid.Empty);

            var segment = await segmentGrain.GeSegmentAsync(ProductId);

            this.State = segment;

            
            await this.WriteStateAsync();
        }
    }
}