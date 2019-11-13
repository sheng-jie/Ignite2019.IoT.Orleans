using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Ignite2019.IoT.Orleans.Model;


namespace Ignite2019.IoT.Orleans.ViewModel.SegmentVMs
{
    public partial class SegmentSearcher : BaseSearcher
    {
        public List<ComboSelectListItem> AllProducts { get; set; }
        [Display(Name = "产品")]
        public int? ProductId { get; set; }

        protected override void InitVM()
        {
            AllProducts = DC.Set<Product>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
        }

    }
}
