using Oms.Domain.Enums;
using Oms.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.Models
{
    /// <summary>
    /// 自定义下单
    /// </summary>
    public class OmsCustomOrderForm
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 租户id
        /// </summary>
        [Required]
        public Guid TenantId { get; set; }

        /// <summary>
        /// 第三方订单编号（微信/支付宝/其他）
        /// </summary>
        [StringLength(50)]
        public string PlatformOrderNo { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        [StringLength(50)]
        public string Source { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        /// <summary>
        /// 购买账号
        /// </summary>
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        /// <summary>
        /// 支付平台的用户id
        /// </summary>
        [StringLength(50)]
        public string PlatformPayerId { get; set; }

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
        public OmsOrderStateEnum State { get; set; }

        /// <summary>
        /// 付款状态
        /// </summary>
        [Required]
        public OmsOrderPayStateEnum PayState { get; set; }

        /// <summary>
        /// 发货状态
        /// </summary>
        [Required]
        public OmsOrderShippingStateEnum ShippingState { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        [Required]
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 实付
        /// </summary>
        [Required]
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// CNY：人民币，境内商户号仅支持人民币
        /// </summary>
        [StringLength(20)]
        public string Currency { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// 抵扣金额
        /// </summary>
        public decimal OffsetAmount { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        [StringLength(20)]
        public string PayType { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 配送方式
        /// </summary>
        [StringLength(20)]
        public string ShippingMethod { get; set; }

        /// <summary>
        /// 快递费
        /// </summary>
        public decimal ShippingPrice { get; set; }

        /// <summary>
        /// 已开发票
        /// </summary>
        [Required]
        public bool IssuedInvoice { get; set; }

        /// <summary>
        /// 预计失效时间
        /// </summary>
        public DateTime? MayFailureTime { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        [StringLength(500)]
        public string Remark { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        public OmsOrderReceiverVo Receiver { get; set; }

        /// <summary>
        /// 其他费用明细
        /// </summary>
        public List<OmsOrderOtherPriceVo> OtherPrices { get; set; } = new List<OmsOrderOtherPriceVo>();

        /// <summary>
        /// 订单明细
        /// </summary>
        [Required]
        public List<OmsOrderItemForm> Items { get; set; } = new List<OmsOrderItemForm>();
    }
}
