using System.ComponentModel.DataAnnotations;

namespace Ignite2019.IoT.Orleans.Model
{
    public enum ProductType
    {
        [Display(Name = "锁")]
        Lock,
        [Display(Name = "保险箱")]
        Box,
        [Display(Name = "灯")]
        Light
    }
}