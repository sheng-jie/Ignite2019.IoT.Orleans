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
        Light,
        [Display(Name="音箱")]
        VoiceBox,
        [Display(Name = "手环")]
        Watch,
        [Display(Name = "窗帘")]
        Curtain
    }
}
