using System.Linq;
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
        public DeviceUniqueIdGenerator()
        {
            this.DataContext = new DataContext("Default", DBTypeEnum.SqlServer);
        }

        public override async Task OnActivateAsync()
        {
            await LoadSegment();
        }

        public async Task<string> NewId()
        {
            await LoadSegment();
            ulong newNum = 0;

            if (this.State.HasRemain)
            {
                newNum = this.State.MaxNum - this.State.Remain - 1;
                this.State.Remain--;
            }

            var newId = $"{newNum:X16}";
            return newId;
        }

        private async Task LoadSegment()
        {
            var productId = this.GetPrimaryKeyLong();
            // get assigned segments of product.
            if (!this.State.HasRemain)
            {
                var availableSegment =
                    this.DataContext.Segments.FirstOrDefault(sg => sg.ProductId == productId && sg.HasRemain);
                if (availableSegment == null)
                {
                    var maxSegment = this.DataContext.Segments.Max(s => s.MaxNum);
                    var newSegment =
                        await this.DataContext.Segments.AddAsync(Segment.AddNewSegment((int)productId, maxSegment));
                    this.State = newSegment.Entity;
                }
                else
                {
                    this.State = availableSegment;
                }
            }
        }

        public override async Task OnDeactivateAsync()
        {
            await this.WriteStateAsync();
            this.DataContext.Segments.Update(this.State);
            await base.OnDeactivateAsync();
        }
    }
}