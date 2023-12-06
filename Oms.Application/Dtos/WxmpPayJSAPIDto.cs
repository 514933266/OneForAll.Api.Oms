using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Application.Dtos
{
    /// <summary>
    /// 发起微信支付
    /// </summary>
    public class WxmpPayJSAPIDto
    {
        /// <summary>
        /// timeStamp
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string NonceStr { get; set; }

        /// <summary>
        /// 小程序下单接口返回的prepay_id参数值，提交格式如：prepay_id=***
        /// </summary>
        public string Package { get; set; }

        /// <summary>
        /// signType
        /// </summary>
        public string SignType { get; set; }

        /// <summary>
        /// signType
        /// </summary>
        public string PaySign { get; set; }
    }
}
