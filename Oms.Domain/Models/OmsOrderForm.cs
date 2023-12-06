using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Oms.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.Models
{
    /// <summary>
    /// 订单
    /// </summary>
    public class OmsOrderForm
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Source { get; set; }

        /// <summary>
        /// 登录用户
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// 商品id
        /// </summary>
        [Required]
        public Guid ProductId { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        [Required]
        public OmsOrderStateEnum OrderState { get; set; } = OmsOrderStateEnum.WaitingPay;

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
        /// 折扣
        /// </summary>
        [Required]
        public decimal Discount { get; set; }

        /// <summary>
        /// 抵扣金额
        /// </summary>
        [Required]
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
        public decimal ShippingPrice { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string AddressJson { get; set; }

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
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Remark { get; set; } = "";
    }

    /// <summary>
    /// 订单明细
    /// </summary>
    public class OmsOrderItemForm
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 订单id
        /// </summary>
        [Required]
        public Guid OrderId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Required]
        [StringLength(200)]
        public string ProductName { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// 商品快照
        /// </summary>
        [Required]
        [StringLength(500)]
        public string ProductSnapshot { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [Required]
        public string UnitPrice { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        [Required]
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        [Required]
        public decimal Discount { get; set; }

        /// <summary>
        /// 抵扣金额
        /// </summary>
        [Required]
        public decimal OffsetAmount { get; set; }

        /// <summary>
        /// 其他费用明细
        /// </summary>
        [Required]
        public string OtherPriceJson { get; set; }
    }
}
