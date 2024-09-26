using System;
using System.Security.Cryptography;
using Oms.Public.Models;
using OneForAll.Core.Utility;
using OneForAll.File;

namespace Oms.Public
{
    /// <summary>
    /// 微信支付SDK
    /// </summary>
    public static class WxPayV3SDK
    {
        /// <summary>
        /// 获取JSAPI下单签名
        /// </summary>
        /// <param name="mchid">商户id</param>
        /// <param name="serialNo">证书序列号</param>
        /// <param name="body">请求体</param>
        /// <returns></returns>
        public static string GetJSAPIOrderSign(string mchid, string serialNo, string privateKey, string body)
        {
            var method = "POST";
            var url = "/v3/pay/transactions/jsapi";
            return GetSign(method, url, mchid, serialNo, privateKey, body);
        }

        /// <summary>
        /// 获取下载商户证书签名
        /// </summary>
        /// <param name="mchid">商户id</param>
        /// <param name="serialNo">证书序列号</param>
        /// <param name="body">请求体</param>
        /// <returns></returns>
        public static string GetCertSign(string mchid, string serialNo, string privateKey, string body)
        {
            var method = "GET";
            var url = "/v3/certificates";
            return GetSign(method, url, mchid, serialNo, privateKey, body);
        }

        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="mchid"></param>
        /// <param name="serialNo"></param>
        /// <param name="privateKey"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static string GetSign(string method, string url, string mchid, string serialNo, string privateKey, string body)
        {
            var tt = TimeHelper.ToTimeStamp().ToString();
            var nonceStr = StringHelper.GetRandomString(32);
            var sign = GetSha256WithRSASign(privateKey, $"{method}\n{url}\n{tt}\n{nonceStr}\n{(body.Length > 0 ? body + "\n" : body)}");
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
        /// 获取微信JSAPI发起支付数据
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="prepayId"></param>
        /// <returns></returns>
        public static WxJSAPIPayData GetWxJSAPIPayData(string appId, string prepayId, string privateKey)
        {
            var tt = TimeHelper.ToTimeStamp().ToString();
            var nonceStr = StringHelper.GetRandomString(32);
            var package = "prepay_id=" + prepayId;
            var sign = GetSha256WithRSASign(privateKey, $"{appId}\n{tt}\n{nonceStr}\n{package}\n");
            return new WxJSAPIPayData()
            {
                AppId = appId,
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
