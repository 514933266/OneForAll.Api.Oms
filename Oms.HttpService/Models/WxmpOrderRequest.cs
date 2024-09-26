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
    ///  微信小程序下单
    /// </summary>
    public class WxmpOrderRequest
    {
        /// <summary>
        /// 由微信生成的应用ID，全局唯一
        /// </summary>
        [Required]
        [JsonProperty("appid")]
        [StringLength(32)]
        public string AppId { get; set; }

        /// <summary>
        /// 直连商户的商户号，由微信支付生成并下发
        /// </summary>
        [Required]
        [JsonProperty("mchid")]
        [StringLength(32)]
        public string Mchid { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        [Required]
        [JsonProperty("description")]
        [StringLength(120)]
        public string Description { get; set; }

        /// <summary>
        /// 商户系统内部订单号，只能是数字、大小写字母_-*且在同一个商户号下唯一
        /// </summary>
        [Required]
        [JsonProperty("out_trade_no")]
        [StringLength(32)]
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 订单失效时间：2015-05-20T13:29:35+08:00
        /// </summary>
        [Required]
        [JsonProperty("time_expire")]
        [StringLength(64)]
        public string TimeExpire { get; set; }

        /// <summary>
        /// 附加数据，在查询API和支付通知中原样返回，可作为自定义参数使用，实际情况下只有支付完成状态才会返回该字段
        /// </summary>
        [Required]
        [JsonProperty("attach")]
        [StringLength(128)]
        public string Attach { get; set; }

        /// <summary>
        /// 异步接收微信支付结果通知的回调地址
        /// </summary>
        [Required]
        [JsonProperty("notify_url")]
        [StringLength(300)]
        public string WxmpNotifyUrl { get; set; }

        /// <summary>
        /// 订单优惠标记
        /// </summary>
        [Required]
        [JsonProperty("goods_tag")]
        [StringLength(32)]
        public string GoodsTag { get; set; } = "";

        /// <summary>
        /// 电子发票入口开放标识，传入true时，支付成功消息和支付详情页将出现开票入口
        /// </summary>
        [Required]
        [JsonProperty("support_fapiao")]
        public bool SupportFapiao { get; set; }

        /// <summary>
        /// 订单金额信息
        /// </summary>
        [Required]
        [JsonProperty("amount")]
        public WxJSAPIOrderAmountRequest Amount { get; set; }

        /// <summary>
        /// 支付者信息
        /// </summary>
        [Required]
        [JsonProperty("payer")]
        public WxJSAPIOrderPayerRequest Payer { get; set; }
    }
}
