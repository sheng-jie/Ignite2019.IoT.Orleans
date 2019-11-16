using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Ignite2019.IoT.Orleans.Model;


namespace Ignite2019.IoT.Orleans.ViewModel.ProductVMs
{
    public partial class ProductListVM : BasePagedListVM<Product_View, ProductSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeStandardAction("Product", GridActionStandardTypesEnum.Create, "新建","", dialogWidth: 800),
                this.MakeStandardAction("Product", GridActionStandardTypesEnum.Edit, "修改","", dialogWidth: 800),
                this.MakeStandardAction("Product", GridActionStandardTypesEnum.Delete, "删除", "",dialogWidth: 800),
                this.MakeStandardAction("Product", GridActionStandardTypesEnum.Details, "详细","", dialogWidth: 800),
                this.MakeStandardAction("Product", GridActionStandardTypesEnum.BatchEdit, "批量修改","", dialogWidth: 800),
                this.MakeStandardAction("Product", GridActionStandardTypesEnum.BatchDelete, "批量删除","", dialogWidth: 800),
                this.MakeStandardAction("Product", GridActionStandardTypesEnum.Import, "导入","", dialogWidth: 800),
                this.MakeStandardAction("Product", GridActionStandardTypesEnum.ExportExcel, "导出",""),
            };
        }

        protected override IEnumerable<IGridColumn<Product_View>> InitGridHeader()
        {
            return new List<GridColumn<Product_View>>{
                this.MakeGridHeader(x => x.Name),
                this.MakeGridHeader(x => x.Description),
                this.MakeGridHeader(x => x.Version),
                this.MakeGridHeader(x => x.ProductType),
                this.MakeGridHeader(x => x.ProtocolType),
                this.MakeGridHeader(x => x.NetType),
                this.MakeGridHeader(x => x.Name_view),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<Product_View> GetSearchQuery()
        {
            var query = DC.Set<Product>()
                .Select(x => new Product_View
                {
				    ID = x.ID,
                    Name = x.Name,
                    Description = x.Description,
                    Version = x.Version,
                    ProductType = x.ProductType,
                    ProtocolType = x.ProtocolType,
                    NetType = x.NetType,
                    Name_view = x.Company.Name,
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class Product_View : Product{
        [Display(Name = "厂家名称")]
        public String Name_view { get; set; }

    }
}
