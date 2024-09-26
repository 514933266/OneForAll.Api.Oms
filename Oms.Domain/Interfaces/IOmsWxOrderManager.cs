using Oms.Domain.AggregateRoots;
using Oms.Public.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.Interfaces
{
    /// <summary>
    /// 微信下单
    /// </summary>
    public interface IOmsWxOrderManager
    {
        /// <summary>
        /// 创建公众号支付订单
        /// </summary>
        /// <param name="setting">商户支付设置</param>
        /// <param name="order">系统订单</param>
        /// <returns>调起微信支付的sign</returns>
        Task<WxJSAPIPayData> CreateWxgzhAsync(OmsWxPaySetting setting, OmsOrder order);

        /// <summary>
        /// 创建JSAPI订单
        /// </summary>
        /// <param name="setting">商户支付设置</param>
        /// <param name="order">系统订单</param>
        /// <returns>调起微信支付的sign</returns>
        Task<WxmpPayData> CreateWxmpAsync(OmsWxPaySetting setting, OmsOrder order);
    }
}
