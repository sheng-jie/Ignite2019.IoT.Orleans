using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Ignite2019.IoT.Orleans.Model;


namespace Ignite2019.IoT.Orleans.ViewModel.EventHistoryVMs
{
    public partial class EventHistoryListVM : BasePagedListVM<EventHistory_View, EventHistorySearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                //this.MakeStandardAction("EventHistory", GridActionStandardTypesEnum.Create, "新建","", dialogWidth: 800),
                //this.MakeStandardAction("EventHistory", GridActionStandardTypesEnum.Edit, "修改","", dialogWidth: 800),
                this.MakeAction("EventHistory", "MockDeviceEvent", "状态变更模拟","模拟状态变更",GridActionParameterTypesEnum.NoId),
                this.MakeStandardAction("EventHistory", GridActionStandardTypesEnum.Delete, "删除", "",dialogWidth: 800),
                this.MakeStandardAction("EventHistory", GridActionStandardTypesEnum.Details, "详细","", dialogWidth: 800),
                //this.MakeStandardAction("EventHistory", GridActionStandardTypesEnum.BatchEdit, "批量修改","", dialogWidth: 800),
                this.MakeStandardAction("EventHistory", GridActionStandardTypesEnum.BatchDelete, "批量删除","", dialogWidth: 800),
                this.MakeStandardAction("EventHistory", GridActionStandardTypesEnum.Import, "导入","", dialogWidth: 800),
                this.MakeStandardAction("EventHistory", GridActionStandardTypesEnum.ExportExcel, "导出",""),
            };
        }

        protected override IEnumerable<IGridColumn<EventHistory_View>> InitGridHeader()
        {
            return new List<GridColumn<EventHistory_View>>{
                this.MakeGridHeader(x => x.DeviceId),
                this.MakeGridHeader(x => x.EventType),
                this.MakeGridHeader(x => x.Detail),
                this.MakeGridHeader(x => x.UpdateTime),
                this.MakeGridHeader(x => x.UserId),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<EventHistory_View> GetSearchQuery()
        {
            var query = DC.Set<EventHistory>()
                .CheckContain(Searcher.DeviceId, x=>x.DeviceId)
                .CheckEqual(Searcher.EventType, x=>x.EventType)
                .CheckEqual(Searcher.UserId, x=>x.UserId)
                .Select(x => new EventHistory_View
                {
				    ID = x.ID,
                    DeviceId = x.DeviceId,
                    EventType = x.EventType,
                    Detail = x.Detail,
                    UpdateTime = x.UpdateTime,
                    UserId = x.UserId,
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class EventHistory_View : EventHistory{

    }
}
