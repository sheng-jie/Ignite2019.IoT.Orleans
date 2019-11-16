using System;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace Ignite2019.IoT.Orleans.Model
{
    /// <summary>
    /// 设备
    /// </summary>
    public class Device : TopBasePoco
    {
        public Device()
        {
            this.CreateTime = DateTime.Now;
        }

        [Key]
        public new string ID { get; set; }

        [Display(Name = "设备名称")]
        public string Name { get; set; }
        [Display(Name = "设备备注")]
        public string Remark { get; set; }

        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }

        [Display(Name = "所属产品")]
        public int ProductId { get; set; }
        [Display(Name = "所属产品")]
        public Product Product { get; set; }
    }
}