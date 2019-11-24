using System;
using System.Linq;
using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.DataAccess;
using Ignite2019.IoT.Orleans.Model;
using Orleans;
using WalkingTec.Mvvm.Core;

namespace Ignite2019.IoT.Orleans.Grains
{
    public class SegmentGrain : Grain, ISegmentGrain
    {
        public DataContext DataContext { get; set; }

        public SegmentGrain()
        {
            this.DataContext = new DataContext("Server=(localdb)\\mssqllocaldb;Database=Orleans_db;Trusted_Connection=True;MultipleActiveResultSets=true", DBTypeEnum.SqlServer);
        }

        public async Task<Segment> GeSegmentAsync(int productId)
        {
            ulong maxSegment = 0x6400000000;
            var hasSegments = this.DataContext.Set<Segment>().Any();

            if (hasSegments)
            {
                var hasProductSegment = this.DataContext.Set<Segment>().Any(sg => sg.ProductId == productId);
                if (hasProductSegment)
                {
                    var availableSegment = this.DataContext.Set<Segment>().FirstOrDefault(sg => sg.ProductId == productId && sg.Remain > 0);
                    if (availableSegment != null)
                    {
                        return availableSegment;
                    }
                }

                maxSegment = this.DataContext.Set<Segment>().Max(s => s.MaxNum);
            }

            var newSegment =
                await this.DataContext.Set<Segment>().AddAsync(Segment.AddNewSegment(productId, maxSegment));
            await this.DataContext.SaveChangesAsync();

            return newSegment.Entity;
        }
    }
}