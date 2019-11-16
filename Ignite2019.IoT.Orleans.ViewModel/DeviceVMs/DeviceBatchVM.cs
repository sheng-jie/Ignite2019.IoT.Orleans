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
    public partial class DeviceBatchVM : BaseBatchVM<Device, Device_BatchEdit>
    {
        public DeviceBatchVM()
        {
            ListVM = new DeviceListVM();
            LinkedVM = new Device_BatchEdit();
        }

    }

	/// <summary>
    /// 批量编辑字段类
    /// </summary>
    public class Device_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
