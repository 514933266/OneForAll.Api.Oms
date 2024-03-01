using Oms.Application.Dtos;
using Oms.Domain.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Application.Interfaces
{
    /// <summary>
    /// 微信小程序支付回调
    /// </summary>
    public interface IOmsWxmpPayCallbackService
    {
        /// <summary>
        /// 回调更新订单状态
        /// </summary>
        /// <param name="settingId">商户设置id</param>
        /// <param name="form">支付回调</param>
        /// <returns></returns>
        Task<OmsWxmpPayCallbackDto> UpdateOrderAsync(Guid settingId, OmsWxmpPayCallbackForm form);
    }
}
