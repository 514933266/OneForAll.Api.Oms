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
    /// 微信商户谁设置
    /// </summary>
    public class OmsWxPaySetting
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 租户id
        /// </summary>
        [Required]
        public Guid SysTenantId { get; set; }

        /// <summary>
        /// 微信的商户号
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Mchid { get; set; }

        /// <summary>
        /// 未支付订单的失效时间：分钟
        /// </summary>
        [Required]
        public int OrderTimeExpire { get; set; } = 2;

        /// <summary>
        /// 下单时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
