using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oms.Domain.Enums;

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
        [StringLength(32)]
        public string PlatformOrderNo { get; set; } = "";

        /// <summary>
        /// 订单来源
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Source { get; set; }

        /// <summary>
        /// 购买账号
        /// </summary>
        [Required]
        [StringLength(20)]
        public string UserName { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Required]
        [StringLength(32)]
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
        public OmsOrderShippingStateEnum ShippingState { get; set; } = OmsOrderShippingStateEnum.None;

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
        public string ShippingMethod { get; set; }

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
        public string ReceiverJson { get; set; }

        /// <summary>
        /// 其他费用明细
        /// </summary>
        [Required]
        public string OtherPriceJson { get; set; }

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
        /// 订单备注
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Remark { get; set; } = "";
    }
}
