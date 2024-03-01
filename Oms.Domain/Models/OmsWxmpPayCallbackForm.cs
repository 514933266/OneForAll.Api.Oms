using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.Models
{
    /// <summary>
    /// 微信小程序支付回调
    /// </summary>
    public class OmsWxmpPayCallbackForm
    {
        /// <summary>
        /// 通知的唯一ID
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 通知创建的时间
        /// </summary>
        [JsonProperty("create_time")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 通知的类型，支付成功通知的类型为TRANSACTION.SUCCESS
        /// </summary>
        [JsonProperty("event_type")]
        public string EventType { get; set; }

        /// <summary>
        /// 通知的资源数据类型，支付成功通知为encrypt-resource
        /// </summary>
        [JsonProperty("resource_type")]
        public string ResourceType { get; set; }

        /// <summary>
        /// 通知资源数据Json
        /// </summary>
        [JsonProperty("resource")]
        public OmsWxmpPayCallbackResourceForm Resource { get; set; }

        /// <summary>
        /// 回调摘要
        /// </summary>
        [JsonProperty("summary")]
        public string Summary { get; set; }
    }

    /// <summary>
    /// 微信小程序支付回调-通知资源数据
    /// </summary>
    public class OmsWxmpPayCallbackResourceForm
    {
        /// <summary>
        /// 对开启结果数据进行加密的加密算法，目前只支持AEAD_AES_256_GCM
        /// </summary>
        [JsonProperty("algorithm")]
        public string Algorithm { get; set; }

        /// <summary>
        /// Base64编码后的开启/停用结果数据密文
        /// </summary>
        [JsonProperty("ciphertext")]
        public string Ciphertext { get; set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        [JsonProperty("associated_data")]
        public string AssociatedData { get; set; }

        /// <summary>
        /// 原始回调类型，为transaction
        /// </summary>
        [JsonProperty("original_type")]
        public string OriginalType { get; set; }

        /// <summary>
        /// 加密使用的随机串
        /// </summary>
        [JsonProperty("nonce")]
        public string Nonce { get; set; }
    }

    /// <summary>
    /// 微信小程序支付回调-订单数据
    /// </summary>
    public class OmsWxmpPayCallbackOrderForm
    {
        /// <summary>
        /// 直连商户申请的公众号或移动应用AppID
        /// </summary>
        [JsonProperty("appid")]
        public string AppId { get; set; }

        /// <summary>
        /// 商户的商户号，由微信支付生成并下发
        /// </summary>
        [JsonProperty("mchid")]
        public string Mchid { get; set; }

        /// <summary>
        /// 商户系统内部订单号，可以是数字、大小写字母_-*的任意组合且在同一个商户号下唯一
        /// </summary>
        [JsonProperty("out_trade_no")]
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 微信支付系统生成的订单号
        /// </summary>
        [JsonProperty("transaction_id")]
        public string TransactionId { get; set; }

        /// <summary>
        /// 交易类型，枚举值
        /// <para>JSAPI：公众号支付</para>
        /// <para>NATIVE：扫码支付</para>
        /// <para>App：App支付</para>
        /// <para>MICROPAY：付款码支付</para>
        /// <para>MWEB：H5支付</para>
        /// <para>FACEPAY：刷脸支付</para>
        /// </summary>
        [JsonProperty("trade_type")]
        public string TradeType { get; set; }

        /// <summary>
        /// 交易状态，枚举值
        /// <para>SUCCESS：支付成功</para>
        /// <para>REFUND：转入退款</para>
        /// <para>NOTPAY：未支付</para>
        /// <para>CLOSED：已关闭</para>
        /// <para>REVOKED：已撤销（付款码支付）</para>
        /// <para>USERPAYING：用户支付中（付款码支付）</para>
        /// <para>PAYERROR：支付失败(其他原因，如银行返回失败)</para>
        /// </summary>
        [JsonProperty("trade_state")]
        public string TradeState { get; set; }

        /// <summary>
        /// 交易状态描述
        /// </summary>
        [JsonProperty("trade_state_des")]
        public string TradeStateDes { get; set; }

        /// <summary>
        /// 银行类型，采用字符串类型的银行标识
        /// </summary>
        [JsonProperty("bank_type")]
        public string BankType { get; set; }

        /// <summary>
        /// 附加数据，在查询API和支付通知中原样返回，可作为自定义参数使用，实际情况下只有支付完成状态才会返回该字段。
        /// </summary>
        [JsonProperty("attach")]
        public string Attach { get; set; }

        /// <summary>
        /// 支付完成时间
        /// </summary>
        [JsonProperty("success_time")]
        public DateTime SuccessTime { get; set; }

        /// <summary>
        /// 支付者信息
        /// </summary>
        [JsonProperty("payer")]
        public OmsWxmpPayCallbackPayerForm Payer { get; set; }

        /// <summary>
        /// 订单金额信息
        /// </summary>
        [JsonProperty("amount")]
        public OmsWxmpPayCallbackAmountForm Amount { get; set; }
    }

    /// <summary>
    /// 订单金额
    /// </summary>
    public class OmsWxmpPayCallbackAmountForm
    {
        /// <summary>
        /// 订单总金额，单位为分
        /// </summary>
        [JsonProperty("total")]
        public int Total { get; set; }

        /// <summary>
        /// CNY：人民币，境内商户号仅支持人民币
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; } = "CNY";
    }

    /// <summary>
    /// 支付者信息
    /// </summary>
    public class OmsWxmpPayCallbackPayerForm
    {
        /// <summary>
        /// 用户在直连商户appid下的唯一标识。 下单前需获取到用户的Openid
        /// </summary>
        [JsonProperty("openid")]
        public string Openid { get; set; }
    }

    /// <summary>
    /// 支付场景信息描述
    /// </summary>
    public class OmsWxmpPayCallbackSceneInfoForm
    {

        /// <summary>
        /// 终端设备号（门店号或收银设备ID）。
        /// </summary>
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }
    }

    /// <summary>
    /// 优惠功能，享受优惠时返回该字段
    /// </summary>
    public class OmsWxmpPayCallbackPromotionDetailForm
    {
        /// <summary>
        /// 券ID
        /// </summary>
        [JsonProperty("coupon_id")]
        public string CouponId { get; set; }

        /// <summary>
        /// 优惠名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 优惠范围，枚举值
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// 优惠类型，枚举值
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// 优惠券面额
        /// </summary>
        [JsonProperty("amount")]
        public int Amount { get; set; }

        /// <summary>
        /// 活动ID
        /// </summary>
        [JsonProperty("stock_id")]
        public string StockId { get; set; }

        /// <summary>
        /// 微信出资，单位为分
        /// </summary>
        [JsonProperty("wechatpay_contribute")]
        public int WechatpayContribute { get; set; }

        /// <summary>
        /// 商户出资，单位为分
        /// </summary>
        [JsonProperty("merchant_contribute")]
        public int MerchantContribute { get; set; }

        /// <summary>
        /// 其他出资，单位为分
        /// </summary>
        [JsonProperty("other_contribute")]
        public int OtherContribute { get; set; }

        /// <summary>
        /// CNY：人民币，境内商户号仅支持人民币
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// 单品列表信息
        /// </summary>
        [JsonProperty("goods_detail")]
        public string GoodsDetail { get; set; }
    }

    /// <summary>
    /// 单品列表信息
    /// </summary>
    public class OmsWxmpPayCallbackPromotionGoodsForm
    {
        /// <summary>
        /// 商品编码
        /// </summary>
        [JsonProperty("goods_id")]
        public string GoodsId { get; set; }

        /// <summary>
        /// 用户购买的数量
        /// </summary>
        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// 商品单价，单位为分
        /// </summary>
        [JsonProperty("unit_price")]
        public int UnitPrice { get; set; }

        /// <summary>
        /// 商品优惠金额
        /// </summary>
        [JsonProperty("discount_amount")]
        public int DiscountAmount { get; set; }

        /// <summary>
        /// 商品备注信息
        /// </summary>
        [JsonProperty("goods_remark")]
        public string GoodsRemark { get; set; }
    }
}
