using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Ignite2019.IoT.Orleans.Model;


namespace Ignite2019.IoT.Orleans.ViewModel.EventHistoryVMs
{
    public partial class EventHistorySearcher : BaseSearcher
    {
        [Display(Name = "设备")]
        public String DeviceId { get; set; }
        [Display(Name = "事件类型")]
        public EventType? EventType { get; set; }
        [Display(Name = "操作用户")]
        public Int32? UserId { get; set; }

        protected override void InitVM()
        {
        }

    }
}
