using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Ignite2019.IoT.Orleans.Model;


namespace Ignite2019.IoT.Orleans.ViewModel.BackgroundJobVMs
{
    public partial class BackgroundJobBatchVM : BaseBatchVM<BackgroundJob, BackgroundJob_BatchEdit>
    {
        public BackgroundJobBatchVM()
        {
            ListVM = new BackgroundJobListVM();
            LinkedVM = new BackgroundJob_BatchEdit();
        }

    }

	/// <summary>
    /// 批量编辑字段类
    /// </summary>
    public class BackgroundJob_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
