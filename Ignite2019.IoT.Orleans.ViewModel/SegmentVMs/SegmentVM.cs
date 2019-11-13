using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Ignite2019.IoT.Orleans.Model;


namespace Ignite2019.IoT.Orleans.ViewModel.SegmentVMs
{
    public partial class SegmentVM : BaseCRUDVM<Segment>
    {
        public List<ComboSelectListItem> AllProducts { get; set; }

        public SegmentVM()
        {
            SetInclude(x => x.Product);
        }

        protected override void InitVM()
        {
            AllProducts = DC.Set<Product>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
        }

        public override void DoAdd()
        {           
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
