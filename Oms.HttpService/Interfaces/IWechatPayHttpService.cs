using Oms.HttpService.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.HttpService.Interfaces
{
    /// <summary>
    /// 微信小程序
    /// </summary>
    public interface IWechatPayHttpService
    {
        /// <summary>
        /// 预创建订单
        /// </summary>
        /// <param name="request">实体</param>
        /// <returns>微信的payid，用于调起支付</returns>
        Task<BaseMessage> CreateJSAPIOrderAsync(WxJSAPIOrderRequest request);

        /// <summary>
        /// 预创建Wxmp订单
        /// </summary>
        /// <param name="mchid">商户id</param>
        /// <param name="serialNo">证书序列号</param>
        /// <param name="privateKey">api v3秘钥</param>
        /// <param name="request">实体</param>
        /// <returns>微信的payid，用于调起支付</returns>
        Task<BaseMessage> CreateWxmpOrderAsync(string mchid, string serialNo, string privateKey, WxmpOrderRequest request);
    }
}
