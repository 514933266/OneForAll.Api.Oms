using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.AggregateRoots
{
    /// <summary>
    /// 订单明细
    /// </summary>
    public class OmsOrderItem
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 订单id
        /// </summary>
        [Required]
        public Guid OmsOrderId { get; set; }

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
        [Required]
        [StringLength(1000)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// 商品快照
        /// </summary>
        [Required]
        public string ProductSnapshotJson { get; set; } = "";

        /// <summary>
        /// 商品数量
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,3)")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,3)")]
        public decimal TotalPrice { get; set; }

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
        /// 其他费用明细
        /// </summary>
        [Required]
        public string OtherPriceJson { get; set; }
    }
}
