using System.ComponentModel.DataAnnotations;

namespace Ignite2019.IoT.Orleans.Model
{
    public enum EventType
    {
        [Display(Name = "上线")]
        Online,
        [Display(Name = "下线")]
        Offline,
        [Display(Name = "状态上报")]
        Report,
        [Display(Name = "指令下发")]
        Control,
    }
}