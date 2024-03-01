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
using OneForAll.Core.DDD;
using Castle.Core.Internal;
using Oms.HttpService.Interfaces;

namespace Oms.Host.Controllers
{
    /// <summary>
    /// 微信小程序支付
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleType.ADMIN)]
    public class OmsWxmpPayController : BaseController
    {
        private readonly IOmsWxmpPayService _service;
        public OmsWxmpPayController(IOmsWxmpPayService service)
        {
            _service = service;
        }

        /// <summary>
        /// 预创建订单，返回prepay_id调起小程序支付
        /// </summary>
        /// <param name="form">订单</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseMessage> CreateOrderAsync([FromBody] OmsOrderForm form)
        {
            return await _service.CreateOrderAsync(form);
        }
    }
}
