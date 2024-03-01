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
    public class OmsOrderForm
    {
        /// <summary>
        /// 订单id(第一次支付可以不携带此参数)
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 租户id(商品所属的租户id)
        /// </summary>
        public Guid TenantId { get; set; }

        /// <summary>
        /// 指定商户号
        /// </summary>
        public string Mchid { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        [StringLength(50)]
        public string Source { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Required]
        [StringLength(50)]
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
        /// 支付方式
        /// </summary>
        [StringLength(20)]
        public string PayType { get; set; }

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
        public List<OmsOrderOtherPriceVo> OtherPrices { get; set; } = new List<OmsOrderOtherPriceVo>();

        /// <summary>
        /// 订单备注
        /// </summary>
        [StringLength(500)]
        public string Remark { get; set; }

        /// <summary>
        /// 订单明细
        /// </summary>
        [Required]
        public List<OmsOrderItemForm> Items { get; set; } = new List<OmsOrderItemForm>();

    }
}
