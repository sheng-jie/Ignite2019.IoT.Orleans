using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Ignite2019.IoT.Orleans.Model;


namespace Ignite2019.IoT.Orleans.ViewModel.DeviceVMs
{
    public partial class DeviceSearcher : BaseSearcher
    {
        public String ID { get; set; }
        [Display(Name = "设备名称")]
        public String Name { get; set; }
        [Display(Name = "设备备注")]
        public String Remark { get; set; }
        [Display(Name = "创建时间")]
        public DateTime? CreateTime { get; set; }
        public List<ComboSelectListItem> AllProducts { get; set; }
        [Display(Name = "所属产品")]
        public int? ProductId { get; set; }

        protected override void InitVM()
        {
            AllProducts = DC.Set<Product>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
        }

    }
}
