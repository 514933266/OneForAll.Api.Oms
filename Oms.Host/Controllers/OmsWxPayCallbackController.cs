using System;
using System.Collections.Generic;
using OneForAll.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Oms.Host.Models;
using Oms.Domain.Models;
using Oms.Application.Dtos;
using Oms.Application.Interfaces;
using Oms.Public.Models;
using Oms.HttpService.Interfaces;
using OneForAll.Core.Extension;

namespace Oms.Host.Controllers
{
    /// <summary>
    /// 微信支付回调
    /// </summary>  
    [Route("open-api/wxpay/callback")]
    public class OmsWxPayCallbackController : BaseController
    {
        private readonly IOmsWxPayCallbackService _service;
        public OmsWxPayCallbackController(IOmsWxPayCallbackService service)
        {
            _service = service;
        }

        /// <summary>
        /// 小程序回调更新订单状态
        /// </summary>
        /// <param name="settingId">设置id</param>
        /// <param name="form">支付回调</param>
        /// <returns></returns>
        [HttpPost]
        [Route("wxmp/{settingId}")]
        public async Task<OmsWxPayCallbackDto> UpdateOrderAsync(Guid settingId, [FromBody] OmsWxmpPayCallbackForm form)
        {
            return await _service.UpdateOrderAsync(settingId, form);
        }
    }
}
