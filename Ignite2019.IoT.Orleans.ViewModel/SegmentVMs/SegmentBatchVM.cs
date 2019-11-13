using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Ignite2019.IoT.Orleans.Model;


namespace Ignite2019.IoT.Orleans.ViewModel.SegmentVMs
{
    public partial class SegmentBatchVM : BaseBatchVM<Segment, Segment_BatchEdit>
    {
        public SegmentBatchVM()
        {
            ListVM = new SegmentListVM();
            LinkedVM = new Segment_BatchEdit();
        }

    }

	/// <summary>
    /// 批量编辑字段类
    /// </summary>
    public class Segment_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
