using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Ignite2019.IoT.Orleans.Model;


namespace Ignite2019.IoT.Orleans.ViewModel.SegmentVMs
{
    public partial class SegmentListVM : BasePagedListVM<Segment_View, SegmentSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeStandardAction("Segment", GridActionStandardTypesEnum.Create, "新建","", dialogWidth: 800),
                this.MakeStandardAction("Segment", GridActionStandardTypesEnum.Edit, "修改","", dialogWidth: 800),
                this.MakeStandardAction("Segment", GridActionStandardTypesEnum.Delete, "删除", "",dialogWidth: 800),
                this.MakeStandardAction("Segment", GridActionStandardTypesEnum.Details, "详细","", dialogWidth: 800),
                this.MakeStandardAction("Segment", GridActionStandardTypesEnum.BatchEdit, "批量修改","", dialogWidth: 800),
                this.MakeStandardAction("Segment", GridActionStandardTypesEnum.BatchDelete, "批量删除","", dialogWidth: 800),
                this.MakeStandardAction("Segment", GridActionStandardTypesEnum.Import, "导入","", dialogWidth: 800),
                this.MakeStandardAction("Segment", GridActionStandardTypesEnum.ExportExcel, "导出",""),
            };
        }

        protected override IEnumerable<IGridColumn<Segment_View>> InitGridHeader()
        {
            return new List<GridColumn<Segment_View>>{
                this.MakeGridHeader(x => x.InitialNum),
                this.MakeGridHeader(x => x.MaxNum),
                this.MakeGridHeader(x => x.Remain),
                this.MakeGridHeader(x => x.Name_view),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<Segment_View> GetSearchQuery()
        {
            var query = DC.Set<Segment>()
                .CheckEqual(Searcher.ProductId, x=>x.ProductId)
                .Select(x => new Segment_View
                {
				    ID = x.ID,
                    InitialNum = x.InitialNum,
                    MaxNum = x.MaxNum,
                    Remain = x.Remain,
                    Name_view = x.Product.Name,
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class Segment_View : Segment{
        [Display(Name = "产品名称")]
        public String Name_view { get; set; }

    }
}
