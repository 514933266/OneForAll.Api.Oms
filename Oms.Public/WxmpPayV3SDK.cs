using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Oms.Public.Models;
using OneForAll.Core.Utility;
using OneForAll.File;

namespace Oms.Public
{
    /// <summary>
    /// 微信支付SDK
    /// </summary>
    public static class WxmpPayV3SDK
    {
        /// <summary>
        /// 获取微信小程序下单签名
        /// </summary>
        /// <param name="mchid">商户id</param>
        /// <param name="serialNo">证书序列号</param>
        /// <param name="body">请求体</param>
        /// <returns></returns>
        public static string GetWxmpCreateOrderSign(string mchid, string serialNo, string privateKey, string body)
        {
            var method = "POST";
            var url = "/v3/pay/transactions/jsapi";
            var tt = TimeHelper.ToTimeStamp().ToString();
            var nonceStr = StringHelper.GetRandomString(32);
            var sign = GetSha256WithRSASign(privateKey, $"{method}\n{url}\n{tt}\n{nonceStr}\n{body}\n");
            return $"WECHATPAY2-SHA256-RSA2048 mchid=\"{mchid}\",serial_no=\"{serialNo}\",timestamp=\"{tt}\",nonce_str=\"{nonceStr}\",signature=\"{sign}\"";
        }

        /// <summary>
        /// 获取微信小程序发起支付数据
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="prepayId"></param>
        /// <returns></returns>
        public static WxmpPayData GetWxmpPayData(string appId, string prepayId, string privateKey)
        {
            var tt = TimeHelper.ToTimeStamp().ToString();
            var nonceStr = StringHelper.GetRandomString(32);
            var package = "prepay_id=" + prepayId;
            var sign = GetSha256WithRSASign(privateKey, $"{appId}\n{tt}\n{nonceStr}\n{package}\n");
            return new WxmpPayData()
            {
                TimeStamp = tt,
                NonceStr = nonceStr,
                Package = package,
                PaySign = sign
            };
        }

        /// <summary>
        /// 获取微信商户证书私钥
        /// </summary>
        /// <param name="filename">证书绝对路径</param>
        /// <returns></returns>
        public static string GetCertPrivateKey(string filename)
        {
            var text = TextHelper.ReadAllText(filename);
            var begin = "-----BEGIN PRIVATE KEY-----\n";
            var end = "\n-----END PRIVATE KEY-----\n";
            return text.Replace(begin, "").Replace(end, "");
        }

        // V3支付Sha256 RSA加密签名
        private static string GetSha256WithRSASign(string privateKey, string message)
        {
            // NOTE： 私钥不包括私钥文件起始的-----BEGIN PRIVATE KEY-----
            //        亦不包括结尾的-----END PRIVATE KEY-----
            byte[] keyData = Convert.FromBase64String(privateKey);

            var rsa = RSA.Create();
            //适用该方法的版本https://learn.microsoft.com/zh-cn/dotnet/api/system.security.cryptography.asymmetricalgorithm.importpkcs8privatekey?view=net-7.0
            rsa.ImportPkcs8PrivateKey(keyData, out _);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
            return Convert.ToBase64String(rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
        }
    }
}
