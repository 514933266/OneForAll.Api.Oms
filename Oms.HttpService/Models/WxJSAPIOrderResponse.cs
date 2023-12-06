using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.HttpService.Models
{
    /// <summary>
    /// 微信JSAPI下单
    /// </summary>
    public class WxJSAPIOrderResponse
    {
        /// <summary>
        /// 预支付交易会话标识。用于后续接口调用中使用，该值有效期为2小时
        /// </summary>
        [Required]
        [JsonProperty("prepay_id")]
        [StringLength(64)]
        public string PrepayId { get; set; }
    }
}
