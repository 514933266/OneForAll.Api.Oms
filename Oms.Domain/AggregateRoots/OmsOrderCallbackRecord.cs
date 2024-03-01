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
    /// 订单回调记录
    /// </summary>
    public class OmsOrderCallbackRecord
    {
        /// <summary>
        /// 订单id
        /// </summary>
        [Key]
        [Required]
        public Guid OmsOrderId { get; set; }

        /// <summary>
        /// 回调地址
        /// </summary>
        [Required]
        public string CallBackUrl { get; set; }

        /// <summary>
        /// 是否回调成功
        /// </summary>
        [Required]
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 成功时间
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? SuccessTime { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        [Required]
        [StringLength(2000)]
        public string Error { get; set; } = "";

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后请求时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime LastUpdateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 时间步长（分钟：重新请求需要叠加的时间）
        /// </summary>
        [Required]
        public int TimeStep { get; set; }

        /// <summary>
        /// 设置步长
        /// </summary>
        public void SetTimeStep()
        {
            switch (TimeStep)
            {
                case 0: TimeStep = 5; break;
                case 5: TimeStep = 10; break;
                case 10: TimeStep = 15; break;
                case 15: TimeStep = 30; break;
                case 30: TimeStep = 60; break;
                case 60: TimeStep = 120; break;
                case 120: TimeStep = 240; break;
                case 240: TimeStep = 360; break;
                default: TimeStep = TimeStep * 2; break;
            }
        }
    }
}
