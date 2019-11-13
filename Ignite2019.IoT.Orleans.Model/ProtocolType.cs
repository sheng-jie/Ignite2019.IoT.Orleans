using System.ComponentModel.DataAnnotations;

namespace Ignite2019.IoT.Orleans.Model
{
    /// <summary>
    /// 协议类型
    /// </summary>
    public enum ProtocolType
    {
        [Display(Name = "MQTT")]
        MQTT,
        [Display(Name = "CoAP")]
        CoAP
    }
}