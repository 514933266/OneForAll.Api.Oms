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
using Oms.Domain.Enums;

namespace Oms.Host.Controllers
{
    /// <summary>
    /// 用户订单
    /// </summary>
    [Route("open-api/[controller]")]
    public class OmsCustomerOrdersController : BaseController
    {
        private readonly IOmsCustomerOrderService _service;

        public OmsCustomerOrdersController(IOmsCustomerOrderService service)
        {
            _service = service;
        }

        /// <summary>
        /// 查询指定id订单
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{orderId}")]
        public async Task<OmsOrderDto> GetAsync(Guid orderId)
        {
            return await _service.GetAsync(orderId);
        }

        /// <summary>
        /// 查询订单列表
        /// </summary>
        /// <param name="orderNos">订单编号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<OmsOrderDto>> GetListAsync([FromQuery] List<string> orderNos)
        {
            return await _service.GetListAsync(orderNos);
        }

        /// <summary>
        /// 查询指定订单列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="userName">购买账号</param>
        /// <param name="state">订单状态</param>
        /// <param name="payState">支付状态（默认显示待支付）</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{pageIndex}/{pageSize}")]
        public async Task<PageList<OmsOrderDto>> GetPgaeAsync(
            int pageIndex,
            int pageSize,
            [FromQuery] string userName,
            [FromQuery] OmsOrderStateEnum? state,
            [FromQuery] OmsOrderPayStateEnum? payState)
        {
            return await _service.GetPgaeAsync(pageIndex, pageSize, userName, state, payState);
        }
    }
}