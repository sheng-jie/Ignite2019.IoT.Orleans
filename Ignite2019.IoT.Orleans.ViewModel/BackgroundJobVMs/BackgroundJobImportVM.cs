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
    public partial class BackgroundJobTemplateVM : BaseTemplateVM
    {

	    protected override void InitVM()
        {
        }

    }

    public class BackgroundJobImportVM : BaseImportVM<BackgroundJobTemplateVM, BackgroundJob>
    {

    }

}
