using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Application.Dtos
{
    /// <summary>
    /// 微信支付回调响应
    /// </summary>
    public class OmsWxPayCallbackDto
    {
        /// <summary>
        /// 返回状态码,错误码，SUCCESS为清算机构接收成功，其他错误码为失败。
        /// </summary>
        [Required]
        [JsonProperty("code")]
        public string Code { set; get; } = "SUCCESS";

        /// <summary>
        /// 返回信息，如非空，为错误原因。
        /// </summary>
        [Required]
        [JsonProperty("message")]
        public string Message { set; get; } = string.Empty;
    }
}
