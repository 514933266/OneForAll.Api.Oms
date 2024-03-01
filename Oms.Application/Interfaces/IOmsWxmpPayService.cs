using Oms.Application.Dtos;
using Oms.Domain.Models;
using Oms.Public.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Application.Interfaces
{
    /// <summary>
    /// 微信小程序支付
    /// </summary>
    public interface IOmsWxmpPayService
    {
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>微信小程序调起JSAPI支付参数</returns>
        Task<BaseMessage> CreateOrderAsync(OmsOrderForm form);
    }
}
