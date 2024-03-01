using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    /// 订单回调更新
    /// </summary>
    public class OmsOrderCallbackRecordUpdateForm
    {
        /// <summary>
        /// 订单id
        /// </summary>
        [Required]
        public Guid OrderId { get; set; }

        /// <summary>
        /// 是否回调成功
        /// </summary>
        [Required]
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        [Required]
        [StringLength(2000)]
        public string Error { get; set; } = "";
    }
}
