using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Public.Models
{
    /// <summary>
    /// 微信小程序发起支付数据
    /// </summary>
    public class WxmpPayData
    {
        /// <summary>
        /// timeStamp
        /// </summary>
        [Required]
        [JsonProperty("timeStamp")]
        public string TimeStamp { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        [Required]
        [JsonProperty("nonceStr")]
        public string NonceStr { get; set; }

        /// <summary>
        /// 小程序下单接口返回的prepay_id参数值，提交格式如：prepay_id=***
        /// </summary>
        [Required]
        [JsonProperty("package")]
        public string Package { get; set; }

        /// <summary>
        /// signType
        /// </summary>
        [Required]
        [JsonProperty("signType")]
        public string SignType { get; set; } = "RSA";

        /// <summary>
        /// signType
        /// </summary>
        [Required]
        [JsonProperty("paySign")]
        public string PaySign { get; set; }
    }
}
