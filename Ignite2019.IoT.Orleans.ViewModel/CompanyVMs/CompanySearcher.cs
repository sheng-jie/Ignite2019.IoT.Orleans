using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Ignite2019.IoT.Orleans.Model;


namespace Ignite2019.IoT.Orleans.ViewModel.CompanyVMs
{
    public partial class CompanySearcher : BaseSearcher
    {
        [Display(Name = "厂家名称")]
        public String Name { get; set; }

        protected override void InitVM()
        {
        }

    }
}
