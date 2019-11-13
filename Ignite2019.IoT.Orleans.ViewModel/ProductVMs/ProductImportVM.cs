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
    public partial class ProductTemplateVM : BaseTemplateVM
    {
        [Display(Name = "产品名称")]
        public ExcelPropety Name_Excel = ExcelPropety.CreateProperty<Product>(x => x.Name);
        [Display(Name = "产品描述")]
        public ExcelPropety Description_Excel = ExcelPropety.CreateProperty<Product>(x => x.Description);
        [Display(Name = "产品版本")]
        public ExcelPropety Version_Excel = ExcelPropety.CreateProperty<Product>(x => x.Version);
        [Display(Name = "产品类型")]
        public ExcelPropety ProductType_Excel = ExcelPropety.CreateProperty<Product>(x => x.ProductType);
        [Display(Name = "协议类型")]
        public ExcelPropety ProtocolType_Excel = ExcelPropety.CreateProperty<Product>(x => x.ProtocolType);
        [Display(Name = "联网方式")]
        public ExcelPropety NetType_Excel = ExcelPropety.CreateProperty<Product>(x => x.NetType);
        [Display(Name = "厂家")]
        public ExcelPropety Company_Excel = ExcelPropety.CreateProperty<Product>(x => x.CompanyId);

	    protected override void InitVM()
        {
            Company_Excel.DataType = ColumnDataType.ComboBox;
            Company_Excel.ListItems = DC.Set<Company>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
        }

    }

    public class ProductImportVM : BaseImportVM<ProductTemplateVM, Product>
    {

    }

}
