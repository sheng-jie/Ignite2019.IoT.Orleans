using System;
using Ignite2019.IoT.Orleans.Model;

namespace Ignite2019.IoT.Orleans.States
{   
    public class SegmentState
    {
        public Guid SegmentId { get; private set; }

        public ulong InitialNum { get; private set; }

        public ulong MaxNum { get; private set; }

        public ulong Remain { get; set; }

        public int ProductId { get; private set; }

        public bool HasRemain => this.Remain > 0;

        public static SegmentState CreateFrom(Segment segment)
        {
            return new SegmentState()
            {
                SegmentId =  segment.ID,
                InitialNum = segment.InitialNum,
                MaxNum = segment.MaxNum,
                ProductId = segment.ProductId,
                Remain = segment.Remain
            };
        }

        public Segment ConvertToSegment()
        {
            return new Segment()
            {
                ID = this.SegmentId,
                InitialNum = this.InitialNum,
                MaxNum = this.MaxNum,
                ProductId = this.ProductId,
                Remain = this.Remain
            };
        }

    }
}