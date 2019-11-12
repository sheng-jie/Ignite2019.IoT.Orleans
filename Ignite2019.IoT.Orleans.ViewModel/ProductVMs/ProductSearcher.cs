using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Ignite2019.IoT.Orleans.Model;


namespace Ignite2019.IoT.Orleans.ViewModel.ProductVMs
{
    public partial class ProductSearcher : BaseSearcher
    {
        [Display(Name = "产品名称")]
        public String Name { get; set; }
        [Display(Name = "产品类型")]
        public ProductType? ProductType { get; set; }
        [Display(Name = "协议类型")]
        public ProtocolType? ProtocolType { get; set; }

        protected override void InitVM()
        {
        }

    }
}
