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

namespace Oms.Host.Controllers
{
    /// <summary>
    /// 微信小程序支付回调
    /// </summary>
    [Route("api/[controller]")]
    public class WxmpPayCallbackController : BaseController
    {
        private readonly IOmsOrderService _service;

        public WxmpPayCallbackController(IOmsOrderService service)
        {
            _service = service;
        }
    }
}
