using Oms.HttpService.Models;
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
        Task<string> CreateOrderAsync(WxJSAPIOrderRequest request);
    }
}
