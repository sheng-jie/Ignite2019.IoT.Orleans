using System.ComponentModel.DataAnnotations;

namespace Ignite2019.IoT.Orleans.Model
{
    public enum JobStatus
    {
        [Display(Name = "未开始")]
        Pending,
        [Display(Name = "运行中")]
        Running,
        [Display(Name = "已中断")]
        Aborted,
        [Display(Name = "已执行")]
        Executed,
        [Display(Name = "已停止")]
        Stopped
    }
}