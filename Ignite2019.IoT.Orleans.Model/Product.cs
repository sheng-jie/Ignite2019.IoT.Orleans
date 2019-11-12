using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WalkingTec.Mvvm.Core;

namespace Ignite2019.IoT.Orleans.Model
{
    /// <summary>
    /// 产品
    /// </summary>
    public class Product: PersistPoco
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new int ID { get; set; }

        [Display(Name = "产品名称")]
        [Required(ErrorMessage = "{0}是必填项")]
        [StringLength(20, ErrorMessage = "{0}最多输入{1}个字符")]

        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "{0}最多输入{1}个字符")]
        [Display(Name = "产品描述")]
        public string Description { get; set; }
        [Display(Name = "产品版本")]
        [StringLength(20, ErrorMessage = "{0}最多输入{1}个字符")]
        public string Version { get; set; }

        [Display(Name = "产品类型")]
        [Required(ErrorMessage = "{0}是必填项")]
        public ProductType ProductType { get; set; }
        [Display(Name = "协议类型")]
        [Required(ErrorMessage = "{0}是必填项")]
        public ProtocolType ProtocolType { get; set; }

        [Display(Name = "段号")]
        public List<Segment> Segments { get; set; }

        [Required]
        public int CompanyId { get; set; }
        public Company Company { get; set; }

    }
}