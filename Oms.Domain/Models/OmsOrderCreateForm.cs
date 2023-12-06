using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    /// 创建订单
    /// </summary>
    public class OmsOrderCreateForm
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        [Required]
        public string ProductName { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        [Required]
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// 抵扣金额
        /// </summary>
        public decimal OffsetAmount { get; set; }

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
        /// 收货地址
        /// </summary>
        public OmsOrderReceiverVo Receiver { get; set; }

        /// <summary>
        /// 其他费用明细
        /// </summary>
        public OmsOrderOtherPriceVo OtherPrice { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        [StringLength(500)]
        public string Remark { get; set; }

        /// <summary>
        /// 订单明细
        /// </summary>
        [Required]
        public List<OmsCreateOrderItemForm> Items { get; set; } = new List<OmsCreateOrderItemForm>();
    }

    /// <summary>
    /// 创建订单明细
    /// </summary>
    public class OmsCreateOrderItemForm
    {
        /// <summary>
        /// 商品编号/id
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ProductNo { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Required]
        [StringLength(200)]
        public string ProductName { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        [StringLength(1000)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// 商品属性快照
        /// </summary>
        [Required]
        public List<OmsProductPropVo> ProductProps { get; set; } = new List<OmsProductPropVo>();

        /// <summary>
        /// 商品数量
        /// </summary>
        [Required]
        [Range(1, 999)]
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
        public decimal Discount { get; set; }

        /// <summary>
        /// 抵扣金额
        /// </summary>
        public decimal OffsetAmount { get; set; }

        /// <summary>
        /// 其他费用明细
        /// </summary>
        public List<OmsOrderOtherPriceVo> OtherPrices { get; set; } = new List<OmsOrderOtherPriceVo>();
    }
}
