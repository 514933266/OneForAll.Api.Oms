using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Application.Dtos
{
    /// <summary>
    /// 微信小程序支付设置
    /// </summary>
    public class OmsWxPaySettingDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 微信的AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// App的名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 微信的商户号
        /// </summary>
        public string Mchid { get; set; }

        /// <summary>
        /// 未支付订单的失效时间：分钟
        /// </summary>
        public int OrderTimeExpire { get; set; }

        /// <summary>
        /// 微信支付回调解密秘钥V3
        /// </summary>
        public string APIv3Key { get; set; }

        /// <summary>
        /// 已上传v3商户证书路径
        /// </summary>
        public bool IsUploadCert { get; set; }

        /// <summary>
        /// 异常提示
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// 创建时间（最后更改时间）
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人名称（最后修改人）
        /// </summary>
        public string CreatorName { get; set; }
    }
}
