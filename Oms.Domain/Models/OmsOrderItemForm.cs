using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Oms.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oms.Domain.ValueObject;

namespace Oms.Domain.Models
{
    /// <summary>
    /// 订单明细
    /// </summary>
    public class OmsOrderItemForm
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
        public List<OmsOrderProductSnapshotVo> ProductSnapshots { get; set; } = new List<OmsOrderProductSnapshotVo>();

        /// <summary>
        /// 商品数量
        /// </summary>
        [Required]
        [Range(1,99999)]
        public int Quantity { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [Required]
        public decimal UnitPrice { get; set; }

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
