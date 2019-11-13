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
        [Display(Name = "联网方式")]
        public NetType? NetType { get; set; }
        public List<ComboSelectListItem> AllCompanys { get; set; }
        [Display(Name = "厂家")]
        public int? CompanyId { get; set; }

        protected override void InitVM()
        {
            AllCompanys = DC.Set<Company>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
        }

    }
}
