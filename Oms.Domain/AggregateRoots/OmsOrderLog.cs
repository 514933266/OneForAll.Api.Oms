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
    /// 订单记录
    /// </summary>
    public class OmsOrderLog
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
        /// 操作人
        /// </summary>
        [Required]
        [StringLength(20)]
        public string CreatorName { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 操作记录Json
        /// </summary>
        [Required]
        public string RecordJson { get; set; }
    }
}
