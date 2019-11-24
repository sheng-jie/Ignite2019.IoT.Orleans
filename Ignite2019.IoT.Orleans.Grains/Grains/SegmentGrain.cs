using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.DataAccess;
using Ignite2019.IoT.Orleans.Model;
using Ignite2019.IoT.Orleans.States;
using Orleans;
using WalkingTec.Mvvm.Core;

namespace Ignite2019.IoT.Orleans.Grains
{
    public class SegmentGrain : Grain<SegmentState>, ISegmentGrain
    {
        public DataContext DataContext { get; set; }

        public int ProductId => (int)this.GetPrimaryKeyLong();
        public SegmentGrain()
        {
            this.DataContext = new DataContext("Server=(localdb)\\mssqllocaldb;Database=Orleans_db;Trusted_Connection=True;MultipleActiveResultSets=true", DBTypeEnum.SqlServer);
        }

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            await LoadSegmentAsync();

            // save state every minutes
            this.RegisterTimer(async o =>
            {
                await this.WriteStateAsync();

            }, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// 申请DeviceId
        /// </summary>
        /// <returns></returns>
        public async Task<string> NewDeviceIdAsync()
        {
            await LoadSegmentAsync();

            var newNum = this.State.MaxNum - this.State.Remain;
            this.State.Remain--;

            return $"{newNum:X16}";
        }

        protected override async Task WriteStateAsync()
        {
            await base.WriteStateAsync();

            var segment = this.State.ConvertToSegment();

            bool tracking = this.DataContext.ChangeTracker.Entries<Segment>().Any(x => x.Entity.ID == segment.ID);
            if (!tracking)
            {
                this.DataContext.Attach(segment);
                //只需要更新Remain 列
                this.DataContext.Entry(segment).Property(p => p.Remain).IsModified = true;
            }

            var state = this.DataContext.Entry(segment);
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

            ulong maxSegment = 0x6400000000;
            var hasSegments = this.DataContext.Set<Segment>().Any();

            if (hasSegments)
            {
                var hasProductSegment = this.DataContext.Set<Segment>().Any(sg => sg.ProductId == ProductId);
                if (hasProductSegment)
                {
                    var availableSegment = this.DataContext.Set<Segment>().FirstOrDefault(sg => sg.ProductId == ProductId && sg.Remain > 0);
                    if (availableSegment != null)
                    {
                        this.State = SegmentState.CreateFrom(availableSegment);
                        return;
                    }
                }

                maxSegment = this.DataContext.Set<Segment>().Max(s => s.MaxNum);
            }

            var newSegment =
                await this.DataContext.Set<Segment>().AddAsync(Segment.AddNewSegment(ProductId, maxSegment));
            await this.DataContext.SaveChangesAsync();

            this.State = SegmentState.CreateFrom(newSegment.Entity);
        }
    }
}