using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.HttpService.Models
{
    /// <summary>
    /// 微信商户证书相应
    /// </summary>
    public class WxV3CertResponse
    {
        /// <summary>
        /// 平台证书的详情，包括此资源的所有定义，业务方需要自行添加
        /// </summary>
        [JsonProperty("data")]
        public List<WxV3CertDataResponse> Data { get; set; }
    }

    /// <summary>
    /// 微信商户证书-平台证书的详情，包括此资源的所有定义，业务方需要自行添加
    /// </summary>
    public class WxV3CertDataResponse
    {
        /// <summary>
        ///【证书序列号】 平台证书的主键，唯一定义此资源的标识
        /// </summary>
        [JsonProperty("serial_no")]
        public string SerialNo { get; set; }

        /// <summary>
        ///【证书启用时间】 启用证书的时间，时间格式为RFC3339。每个平台证书的启用时间是固定的。
        /// </summary>
        [JsonProperty("effective_time")]
        public DateTime EffectiveTime { get; set; }

        /// <summary>
        ///【证书弃用时间】 弃用证书的时间，时间格式为RFC3339。更换平台证书前，会提前24小时修改老证书的弃用时间，接口返回新老两个平台证书。更换完成后，接口会返回最新的平台证书。
        /// </summary>
        [JsonProperty("expire_time")]
        public DateTime ExpireTime { get; set; }

        /// <summary>
        ///【证书信息】 证书内容
        /// </summary>
        [JsonProperty("encrypt_certificate")]
        public WxV3CertEncryptResponse EncryptCertificate { get; set; }
    }

    /// <summary>
    ///【证书信息】 证书内容
    /// </summary>
    public class WxV3CertEncryptResponse
    {
        /// <summary>
        ///【加密证书的算法】 对开启结果数据进行加密的加密算法，目前只支持AEAD_AES_256_GCM。
        /// </summary>
        [JsonProperty("algorithm")]
        public string Algorithm { get; set; }

        /// <summary>
        ///【加密证书的随机串】 对应到加密算法中的IV。
        /// </summary>
        [JsonProperty("nonce")]
        public string Nonce { get; set; }

        /// <summary>
        ///【加密证书的附加数据】 加密证书的附加数据，固定为“certificate"。
        /// </summary>
        [JsonProperty("associated_data")]
        public string AssociatedData { get; set; }

        /// <summary>
        ///【加密后的证书内容】 使用API KEY和上述参数，可以解密出平台证书的明文。证书明文为PEM格式。（注意：更换证书时会出现PEM格式中的证书失效时间与接口返回的证书弃用时间不一致的情况）
        /// </summary>
        [JsonProperty("ciphertext")]
        public string Ciphertext { get; set; }
    }
}
