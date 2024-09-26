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
using OneForAll.Core.DDD;
using Oms.Domain.ValueObject;
using Oms.Host.Filters;
using OneForAll.Core.OAuth;

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

        /// <summary>
        /// 查询指定日期的订单当日金额统计
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{date}/TodayAmount")]
        public async Task<OmsOrderTodayAmountVo> GetTodayAmountAsync(DateTime date)
        {
            return await _service.GetTodayAmountAsync(date);
        }

        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="id">订单id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<OmsOrderDto> GetAsync(Guid id)
        {
            return await _service.GetAsync(id);
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
        /// 获取分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="orderNo">订单编号</param>
        /// <param name="userName">购买用户账号</param>
        /// <param name="source">来源</param>
        /// <param name="platformOrderNo">第三方平台单号</param>
        /// <param name="payType">支付方式</param>
        /// <param name="state">订单状态</param>
        /// <param name="payState">支付状态</param>
        /// <param name="shippingState">物流状态</param>
        /// <param name="createBeginTime">创建开始时间</param>
        /// <param name="createEndTime">创建结束时间</param>
        /// <param name="payBeginTime">支付开始时间</param>
        /// <param name="payEndTime">支付结束时间</param>
        /// <returns>权限列表</returns>
        [HttpGet]
        [Route("{pageIndex}/{pageSize}")]
        [CheckPermission(Action = ConstPermission.EnterView)]
        public async Task<PageList<OmsOrderDto>> GetPageAsync(
            int pageIndex,
            int pageSize,
            [FromQuery] string orderNo = "",
            [FromQuery] string userName = "",
            [FromQuery] string source = "",
            [FromQuery] string platformOrderNo = "",
            [FromQuery] string payType = "",
            [FromQuery] OmsOrderStateEnum? state = null,
            [FromQuery] OmsOrderPayStateEnum? payState = null,
            [FromQuery] OmsOrderShippingStateEnum? shippingState = null,
            [FromQuery] DateTime? createBeginTime = null,
            [FromQuery] DateTime? createEndTime = null,
            [FromQuery] DateTime? payBeginTime = null,
            [FromQuery] DateTime? payEndTime = null)
        {
            return await _service.GetPageAsync(pageIndex, pageSize, new OmsGetPageOrderForm()
            {
                OrderNo = orderNo,
                UserName = userName,
                Source = source,
                PlatformOrderNo = platformOrderNo,
                PayType = payType,
                State = state,
                PayState = payState,
                ShippingState = shippingState,
                CreateBeginTime = createBeginTime,
                CreateEndTime = createEndTime,
                PayBeginTime = payBeginTime,
                PayEndTime = payEndTime
            });
        }

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="form">自定义订单</param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission(Action = ConstPermission.EnterView)]
        public async Task<BaseMessage> AddAsync([FromBody] OmsCustomOrderForm form)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.AddAsync(form);

            switch (msg.ErrType)
            {
                case BaseErrType.Success:
                    msg.Data = form.Id;
                    return msg.Success("添加成功");
                default: return msg.Fail("添加失败");
            }
        }
    }
}
