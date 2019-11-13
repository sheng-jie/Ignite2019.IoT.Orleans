using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace Ignite2019.IoT.Orleans.Model
{
    public class Segment : BasePoco
    {
        [Display(Name = "起始值")]
        public ulong InitialNum { get; set; }

        [Display(Name = "最大值")]
        public ulong MaxNum { get; set; }

        [Display(Name = "可用数量")]
        public ulong Remain { get; set; }

        [Display(Name = "产品")]
        public int ProductId { get; set; }
        [Display(Name = "产品")]
        public Product Product { get; set; }
    }
}