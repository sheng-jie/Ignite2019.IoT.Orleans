using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Ignite2019.IoT.Orleans.Model;


namespace Ignite2019.IoT.Orleans.ViewModel.CompanyVMs
{
    public partial class CompanyBatchVM : BaseBatchVM<Company, Company_BatchEdit>
    {
        public CompanyBatchVM()
        {
            ListVM = new CompanyListVM();
            LinkedVM = new Company_BatchEdit();
        }

    }

	/// <summary>
    /// 批量编辑字段类
    /// </summary>
    public class Company_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
