using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Ignite2019.IoT.Orleans.Model;


namespace Ignite2019.IoT.Orleans.ViewModel.DeviceVMs
{
    public partial class DeviceVM : BaseCRUDVM<Device>
    {
        public List<ComboSelectListItem> AllProducts { get; set; }

        public DeviceVM()
        {
            SetInclude(x => x.Product);
        }

        protected override void InitVM()
        {
            AllProducts = DC.Set<Product>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
        }

        public override void DoAdd()
        {
            this.Entity.Name = string.IsNullOrEmpty(this.Entity.Name)
                ? this.DC.Set<Product>().FirstOrDefault(p => p.ID == this.Entity.ProductId)?.Name
            : this.Entity.Name;
            base.DoAdd();
        }

        public override void DoEdit(bool updateAllFields = false)
        {
            base.DoEdit(updateAllFields);
        }

        public override void DoDelete()
        {
            base.DoDelete();
        }
    }
}
