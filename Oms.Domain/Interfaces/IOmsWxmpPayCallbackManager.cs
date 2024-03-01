using Oms.Domain.AggregateRoots;
using Oms.Domain.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.Interfaces
{
    /// <summary>
    /// 微信小程序支付回调
    /// </summary>
    public interface IOmsWxmpPayCallbackManager
    {
        /// <summary>
        /// 更新订单
        /// </summary>
        /// <param name="setting">商户设置</param>
        /// <param name="wxOrder">微信订单数据</param>
        /// <returns></returns>
        Task<BaseErrType> UpdateOrderAsync(OmsWxPaySetting setting, OmsWxmpPayCallbackOrderForm wxOrder);
    }
}
