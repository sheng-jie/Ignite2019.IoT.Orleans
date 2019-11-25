using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Ignite2019.IoT.Orleans.Model;


namespace Ignite2019.IoT.Orleans.ViewModel.DeviceVMs
{
    public partial class DeviceListVM : BasePagedListVM<Device_View, DeviceSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {

                this.MakeAction("Device","MockBatchCreate","模拟批量创建","模拟批量创建",GridActionParameterTypesEnum.NoId),

                this.MakeAction("EventHistory","Logs","设备日志","设备日志列表",GridActionParameterTypesEnum.SingleId),
                this.MakeStandardAction("Device", GridActionStandardTypesEnum.Create, "新建","", dialogWidth: 800),
                this.MakeStandardAction("Device", GridActionStandardTypesEnum.Edit, "修改","", dialogWidth: 800),
                this.MakeStandardAction("Device", GridActionStandardTypesEnum.Delete, "删除", "",dialogWidth: 800),
                this.MakeStandardAction("Device", GridActionStandardTypesEnum.Details, "详细","", dialogWidth: 800),
                this.MakeStandardAction("Device", GridActionStandardTypesEnum.BatchEdit, "批量修改","", dialogWidth: 800),
                this.MakeStandardAction("Device", GridActionStandardTypesEnum.BatchDelete, "批量删除","", dialogWidth: 800),
                this.MakeStandardAction("Device", GridActionStandardTypesEnum.Import, "导入","", dialogWidth: 800),
                this.MakeStandardAction("Device", GridActionStandardTypesEnum.ExportExcel, "导出",""),
            };
        }

        protected override IEnumerable<IGridColumn<Device_View>> InitGridHeader()
        {
            return new List<GridColumn<Device_View>>{
                this.MakeGridHeader(x => x.ID),
                this.MakeGridHeader(x => x.Name),
                this.MakeGridHeader(x => x.Remark),
                this.MakeGridHeader(x => x.CreateTime),
                this.MakeGridHeader(x => x.Name_view),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<Device_View> GetSearchQuery()
        {
            var query = DC.Set<Device>()
                .CheckContain(Searcher.ID, x => x.ID)
                .CheckContain(Searcher.Name, x => x.Name)
                .CheckContain(Searcher.Remark, x => x.Remark)
                .CheckEqual(Searcher.CreateTime, x => x.CreateTime)
                .CheckEqual(Searcher.ProductId, x => x.ProductId)
                .Select(x => new Device_View
                {
                    ID = x.ID,
                    Name = x.Name,
                    Remark = x.Remark,
                    CreateTime = x.CreateTime,
                    Name_view = x.Product.Name,
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class Device_View : Device
    {
        [Display(Name = "产品名称")]
        public String Name_view { get; set; }

    }
}
