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

namespace Oms.Host.Controllers
{
    /// <summary>
    /// 微信小程序支付
    /// </summary>
    [Route("api/[controller]")]
    //[Authorize(Roles = UserRoleType.ADMIN)]
    public class WxmpPayController : BaseController
    {
        private readonly IWxmpPayService _service;

        public WxmpPayController(IWxmpPayService service)
        {
            _service = service;
        }

        /// <summary>
        /// 预创建订单，返回prepay_id调起小程序支付
        /// </summary>
        /// <param name="form">订单</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseMessage> CreateOrderAsync([FromBody] OmsOrderCreateForm form)
        {
            var msg = new BaseMessage();
            var data = await _service.CreateOrderAsync(LoginUser, form);

            if (data != null)
            {
                msg.ErrType = BaseErrType.Success;
                msg.Data = data;
            }
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("订单创建成功");
                default: return msg.Fail("订单创建失败");
            }
        }
    }
}
