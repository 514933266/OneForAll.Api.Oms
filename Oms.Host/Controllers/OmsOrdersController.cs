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
    /// 订单管理
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleType.ADMIN)]
    public class OmsOrdersController : BaseController
    {
        private readonly IOmsOrderService _service;

        public OmsOrdersController(IOmsOrderService service)
        {
            _service = service;
        }
    }
}
