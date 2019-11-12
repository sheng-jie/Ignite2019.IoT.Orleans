using System.ComponentModel.DataAnnotations;

namespace Ignite2019.IoT.Orleans.Model
{
    /// <summary>
    /// 设备
    /// </summary>
    public class Device
    {
        [Key]
        public new string ID { get; set; }

        public string Name { get; set; }
        public string Remark { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}