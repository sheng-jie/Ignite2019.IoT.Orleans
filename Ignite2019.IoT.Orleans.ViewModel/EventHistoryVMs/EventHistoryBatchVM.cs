using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Ignite2019.IoT.Orleans.Model;


namespace Ignite2019.IoT.Orleans.ViewModel.EventHistoryVMs
{
    public partial class EventHistoryBatchVM : BaseBatchVM<EventHistory, EventHistory_BatchEdit>
    {
        public EventHistoryBatchVM()
        {
            ListVM = new EventHistoryListVM();
            LinkedVM = new EventHistory_BatchEdit();
        }

    }

	/// <summary>
    /// 批量编辑字段类
    /// </summary>
    public class EventHistory_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
