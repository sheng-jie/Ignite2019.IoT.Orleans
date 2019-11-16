using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Ignite2019.IoT.Orleans.Model;


namespace Ignite2019.IoT.Orleans.ViewModel.ProductVMs
{
    public partial class ProductBatchVM : BaseBatchVM<Product, Product_BatchEdit>
    {
        public ProductBatchVM()
        {
            ListVM = new ProductListVM();
            LinkedVM = new Product_BatchEdit();
        }

    }

	/// <summary>
    /// 批量编辑字段类
    /// </summary>
    public class Product_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
