using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.HttpService.Interfaces
{
    /// <summary>
    /// 订单回调
    /// </summary>
    public interface IOrderCallbackHttpService
    {
        /// <summary>
        /// 回传订单信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="order">订单</param>
        /// <returns></returns>
        Task<BaseMessage> SynOrderAsync(string url, object order);
    }
}
