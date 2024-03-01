using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oms.Domain.Enums;
using NPOI.SS.Formula.UDF;
using Oms.Domain.Models;
using Newtonsoft.Json;
using OneForAll.Core.Utility;
using OneForAll.Core.Extension;

namespace Oms.Domain.AggregateRoots
{
    /// <summary>
    /// 订单
    /// </summary>
    public class OmsOrder
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 租户id
        /// </summary>
        [Required]
        public Guid SysTenantId { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [Required]
        public long OrderNo { get; set; }

        /// <summary>
        /// 第三方订单编号（微信/支付宝/其他）
        /// </summary>
        [Required]
        [StringLength(50)]
        public string PlatformOrderNo { get; set; } = "";

        /// <summary>
        /// 订单来源
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Source { get; set; } = "";

        /// <summary>
        /// 用户id
        /// </summary>
        [Required]
        [StringLength(50)]
        public string UserId { get; set; } = "";

        /// <summary>
        /// 购买账号
        /// </summary>
        [Required]
        [StringLength(50)]
        public string UserName { get; set; } = "";

        /// <summary>
        /// 支付平台的用户id
        /// </summary>
        [Required]
        [StringLength(50)]
        public string PlatformPayerId { get; set; } = "";

        /// <summary>
        /// 商品名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        [Required]
        public OmsOrderStateEnum State { get; set; } = OmsOrderStateEnum.WaitingPay;

        /// <summary>
        /// 付款状态
        /// </summary>
        [Required]
        public OmsOrderPayStateEnum PayState { get; set; } = OmsOrderPayStateEnum.UnPay;

        /// <summary>
        /// 发货状态
        /// </summary>
        [Required]
        public OmsOrderShippingStateEnum ShippingState { get; set; } = OmsOrderShippingStateEnum.Pending;

        /// <summary>
        /// 总价
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,3)")]
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 实付
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,3)")]
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// CNY：人民币，境内商户号仅支持人民币
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Currency { get; set; } = "CNY";

        /// <summary>
        /// 折扣
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(4,2)")]
        public decimal Discount { get; set; }

        /// <summary>
        /// 抵扣金额
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,3)")]
        public decimal OffsetAmount { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        [Required]
        [StringLength(20)]
        public string PayType { get; set; } = "";

        /// <summary>
        /// 支付时间
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 配送方式
        /// </summary>
        [Required]
        [StringLength(20)]
        public string ShippingMethod { get; set; } = "无需配送";

        /// <summary>
        /// 快递费
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,3)")]
        public decimal ShippingPrice { get; set; }

        /// <summary>
        /// 收货人信息Json
        /// </summary>
        [Required]
        public string ReceiverJson { get; set; } = "";

        /// <summary>
        /// 其他费用明细
        /// </summary>
        [Required]
        public string OtherPriceJson { get; set; } = "";

        /// <summary>
        /// 已开发票
        /// </summary>
        [Required]
        public bool IssuedInvoice { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后修改时间
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 预计失效时间
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? MayFailureTime { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Remark { get; set; } = "";

        /// <summary>
        /// 生成订单号
        /// </summary>
        public void CreateOrderNo()
        {
            // 后续订单量上来，可将此处替换为雪花算法
            var orderNo = DateTime.Now.ToString("yyyyMMddhhmmss") + StringHelper.GetRandomNumber(4);
            OrderNo = orderNo.TryLong();
        }

        /// <summary>
        /// 同步微信状态
        /// </summary>
        /// <param name="wxOrder">微信支付回调订单内容</param>
        public void SynWxOrder(OmsWxmpPayCallbackOrderForm wxOrder)
        {
            if (wxOrder != null)
            {
                PlatformPayerId = wxOrder.Payer.Openid;
                PlatformOrderNo = wxOrder.TransactionId;
                PayTime = wxOrder.SuccessTime;
                PaidAmount = (wxOrder.Amount.Total / 100);
                if (!wxOrder.Amount.Currency.IsNullOrEmpty())
                    Currency = wxOrder.Amount.Currency;

                switch (wxOrder.TradeType)
                {
                    case "JSAPI":
                        PayType = "公众号/小程序支付";
                        break;
                    case "NATIVE":
                        PayType = "扫码支付";
                        break;
                    case "App":
                        PayType = "App支付";
                        break;
                    case "MICROPAY":
                        PayType = "付款码支付";
                        break;
                    case "MWEB":
                        PayType = "H5支付";
                        break;
                    case "FACEPAY":
                        PayType = "刷脸支付";
                        break;
                    default:
                        PayType = "其他";
                        break;
                }

                switch (wxOrder.TradeState)
                {
                    case "SUCCESS":
                        Paid();
                        break;
                    case "REFUND":
                        State = OmsOrderStateEnum.Refunded;
                        PayState = OmsOrderPayStateEnum.Refunded;
                        break;
                    case "NOTPAY":
                        State = OmsOrderStateEnum.WaitingPay;
                        PayState = OmsOrderPayStateEnum.UnPay; break;
                    case "CLOSED":
                        State = OmsOrderStateEnum.Closed;
                        PayState = OmsOrderPayStateEnum.UnPay;
                        break;
                    case "REVOKED":
                        State = OmsOrderStateEnum.Canceled;
                        PayState = OmsOrderPayStateEnum.UnPay; break;
                    case "USERPAYING":
                        State = OmsOrderStateEnum.WaitingPay;
                        PayState = OmsOrderPayStateEnum.UnPay;
                        break;
                    case "PAYERROR":
                        State = OmsOrderStateEnum.WaitingPay;
                        PayState = OmsOrderPayStateEnum.UnPay; break;
                    default:
                        State = OmsOrderStateEnum.Error;
                        PayState = OmsOrderPayStateEnum.UnPay; break;
                }
            }
        }

        /// <summary>
        /// 设置支付状态
        /// </summary>
        public void Paid()
        {
            if (State != OmsOrderStateEnum.Paid)
                State = ShippingMethod == "无需配送" ? OmsOrderStateEnum.Completed : OmsOrderStateEnum.Paid;
            if (PayState != OmsOrderPayStateEnum.Paid)
                PayState = OmsOrderPayStateEnum.Paid;
            if (!PayTime.HasValue)
                PayTime = DateTime.Now;
            if (!UpdateTime.HasValue)
                UpdateTime = DateTime.Now;
        }
    }
}
