using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Ignite2019.IoT.Orleans.Model;


namespace Ignite2019.IoT.Orleans.ViewModel.BackgroundJobVMs
{
    public partial class BackgroundJobListVM : BasePagedListVM<BackgroundJob_View, BackgroundJobSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeStandardAction("BackgroundJob", GridActionStandardTypesEnum.Create, "新建","", dialogWidth: 800),
                this.MakeStandardAction("BackgroundJob", GridActionStandardTypesEnum.Edit, "修改","", dialogWidth: 800),
                this.MakeStandardAction("BackgroundJob", GridActionStandardTypesEnum.Delete, "删除", "",dialogWidth: 800),
                this.MakeStandardAction("BackgroundJob", GridActionStandardTypesEnum.Details, "详细","", dialogWidth: 800),
                this.MakeStandardAction("BackgroundJob", GridActionStandardTypesEnum.BatchEdit, "批量修改","", dialogWidth: 800),
                this.MakeStandardAction("BackgroundJob", GridActionStandardTypesEnum.BatchDelete, "批量删除","", dialogWidth: 800),
                this.MakeStandardAction("BackgroundJob", GridActionStandardTypesEnum.Import, "导入","", dialogWidth: 800),
                this.MakeStandardAction("BackgroundJob", GridActionStandardTypesEnum.ExportExcel, "导出",""),
            };
        }

        protected override IEnumerable<IGridColumn<BackgroundJob_View>> InitGridHeader()
        {
            return new List<GridColumn<BackgroundJob_View>>{
                this.MakeGridHeader(x => x.DeviceId),
                this.MakeGridHeader(x => x.JobType),
                this.MakeGridHeader(x => x.JobStatus),
                this.MakeGridHeader(x => x.Command),
                this.MakeGridHeader(x => x.StartTime),
                this.MakeGridHeader(x => x.Period),
                this.MakeGridHeader(x => x.EndTime),
                this.MakeGridHeader(x => x.ExecutedCount),
                this.MakeGridHeader(x => x.LastExecuteTime),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<BackgroundJob_View> GetSearchQuery()
        {
            var query = DC.Set<BackgroundJob>()
                .CheckContain(Searcher.DeviceId, x=>x.DeviceId)
                .CheckEqual(Searcher.JobType, x=>x.JobType)
                .Select(x => new BackgroundJob_View
                {
				    ID = x.ID,
                    DeviceId = x.DeviceId,
                    JobType = x.JobType,
                    JobStatus = x.JobStatus,
                    Command = x.Command,
                    StartTime = x.StartTime,
                    Period = x.Period,
                    EndTime = x.EndTime,
                    ExecutedCount = x.ExecutedCount,
                    LastExecuteTime = x.LastExecuteTime,
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class BackgroundJob_View : BackgroundJob{

    }
}
