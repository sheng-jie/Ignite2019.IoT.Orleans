using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WalkingTec.Mvvm.Core;

namespace Ignite2019.IoT.Orleans.Model
{
    /// <summary>
    /// 厂家
    /// </summary>
    public class Company : PersistPoco
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new int ID { get; set; }
        [Display(Name = "厂家名称")]
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}