using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
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
        public int OrderTimeExpire { get; set; } = 30;

        /// <summary>
        /// 微信支付回调解密秘钥V3
        /// </summary>
        [Required]
        [StringLength(50)]
        public string APIv3Key { get; set; }

        /// <summary>
        /// 证书序列号
        /// </summary>
        [Required]
        [StringLength(50)]
        public string CertSerialNo { get; set; } = "";

        /// <summary>
        /// 已上传v3商户证书路径
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string CertificateUrl { get; set; } = "";

        /// <summary>
        /// 已上传v3商户证书秘钥路径
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string CertificateKeyUrl { get; set; } = "";

        /// <summary>
        /// 自动更新支付证书
        /// </summary>
        [Required]
        public bool IsAutoRefreshCert { get; set; }

        /// <summary>
        /// 回调地址，如有会将订单基本信息回传
        /// </summary>
        [Required]
        [StringLength(300)]

        public string CallbackUrl { get; set; } = "";

        /// <summary>
        /// 异常提示
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Error { get; set; } = "";

        /// <summary>
        /// 创建时间（最后更改时间）
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建人id（最后修改人）
        /// </summary>
        [Required]
        public Guid CreatorId { get; set; }

        /// <summary>
        /// 创建人名称（最后修改人）
        /// </summary>
        [Required]
        [StringLength(20)]
        public string CreatorName { get; set; }

        /// <summary>
        /// 检查是否能够发起支付
        /// </summary>
        /// <returns></returns>
        public bool CheckCanRequestPay()
        {
            if (Mchid.IsNullOrEmpty())
                return false;
            if (CertificateKeyUrl.IsNullOrEmpty())
                return false;
            if (CertificateUrl.IsNullOrEmpty())
                return false;
            return true;
        }
    }
}
