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
    /// 微信小程序支付设置
    /// </summary>
    public class OmsWxPaySettingForm
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 微信的AppId
        /// </summary>
        [Required]
        [StringLength(50)]
        public string AppId { get; set; }

        /// <summary>
        /// App的名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string AppName { get; set; }

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
        public int OrderTimeExpire { get; set; }

        /// <summary>
        /// 微信支付回调解密秘钥V3
        /// </summary>
        [Required]
        [StringLength(50)]
        public string APIv3Key { get; set; }

        /// <summary>
        /// 回调地址，如有会将订单基本信息回传
        /// </summary>
        [Required]
        [StringLength(300)]
        public string CallbackUrl { get; set; }
    }
}
