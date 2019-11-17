using System.Linq;
using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.DataAccess;
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
            await this.ReadStateAsync();
            await LoadSegment();
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

            await this.WriteStateAsync();

            var newId = $"{newNum:X16}";

            return newId;
        }

        private async Task LoadSegment()
        {
            var productId = this.GetPrimaryKeyLong();
            // get assigned segments of product.
            if (!this.State.HasRemain)
            {
                ulong maxSegment = 0;
                var hasSegments = this.DataContext.Set<Segment>().Any();

                if (!hasSegments)
                {
                    maxSegment = 0x6400000000;
                }
                else
                {
                    var hasProductSegment = this.DataContext.Set<Segment>().Any(sg => sg.ProductId == (int)productId);
                    if (hasProductSegment)
                    {
                        var availableSegment = this.DataContext.Set<Segment>().FirstOrDefault(sg => sg.ProductId == productId && sg.Remain > 0);
                        if (availableSegment != null)
                        {
                            this.State = availableSegment;
                            return;
                        }
                    }

                    maxSegment = this.DataContext.Set<Segment>().Max(s => s.MaxNum);
                }

                var newSegment =
                    await this.DataContext.Set<Segment>().AddAsync(Segment.AddNewSegment((int)productId, maxSegment));
                await this.DataContext.SaveChangesAsync();
                this.State = newSegment.Entity;
            }
        }

        public override async Task OnDeactivateAsync()
        {
            await this.WriteStateAsync();
            this.DataContext.Set<Segment>().Update(this.State);

            await this.DataContext.SaveChangesAsync();
            await base.OnDeactivateAsync();
        }
    }
}