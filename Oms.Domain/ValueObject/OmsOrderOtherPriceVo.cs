using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.ValueObject
{
    /// <summary>
    /// 订单其他费用
    /// </summary>
    public class OmsOrderOtherPriceVo
    {
        /// <summary>
        /// 费用名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,3)")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Remark { get; set; }
    }
}
