using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oms.Domain.Enums;

namespace Oms.Domain.ValueObject
{
    /// <summary>
    /// 订单日志详情
    /// </summary>
    public class OmsOrderLogDetailVo
    {
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
        /// 操作人
        /// </summary>
        [Required]
        [StringLength(50)]
        public string CreatorName { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 备注
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Remark { get; set; }
    }
}
