using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Ignite2019.IoT.Orleans.Model;


namespace Ignite2019.IoT.Orleans.ViewModel.BackgroundJobVMs
{
    public partial class BackgroundJobSearcher : BaseSearcher
    {
        [Display(Name = "设备Id")]
        public String DeviceId { get; set; }

        protected override void InitVM()
        {
        }

    }
}
