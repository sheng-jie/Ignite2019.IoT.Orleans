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
    public partial class DeviceTemplateVM : BaseTemplateVM
    {
        public ExcelPropety ID_Excel = ExcelPropety.CreateProperty<Device>(x => x.ID);
        [Display(Name = "设备名称")]
        public ExcelPropety Name_Excel = ExcelPropety.CreateProperty<Device>(x => x.Name);
        [Display(Name = "设备备注")]
        public ExcelPropety Remark_Excel = ExcelPropety.CreateProperty<Device>(x => x.Remark);
        [Display(Name = "创建时间")]
        public ExcelPropety CreateTime_Excel = ExcelPropety.CreateProperty<Device>(x => x.CreateTime);
        [Display(Name = "所属产品")]
        public ExcelPropety Product_Excel = ExcelPropety.CreateProperty<Device>(x => x.ProductId);

	    protected override void InitVM()
        {
            Product_Excel.DataType = ColumnDataType.ComboBox;
            Product_Excel.ListItems = DC.Set<Product>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
        }

    }

    public class DeviceImportVM : BaseImportVM<DeviceTemplateVM, Device>
    {

    }

}
