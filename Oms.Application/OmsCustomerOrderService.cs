using AutoMapper;
using Oms.Application.Dtos;
using Oms.Application.Interfaces;
using Oms.Domain.Aggregates;
using Oms.Domain.Enums;
using Oms.Domain.Interfaces;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Application
{
    /// <summary>
    /// 个人订单
    /// </summary>
    public class OmsCustomerOrderService : IOmsPersonalOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOmsPersonalOrderManager _manager;

        public OmsCustomerOrderService(
            IMapper mapper,
            IOmsPersonalOrderManager manager)
        {
            _mapper = mapper;
            _manager = manager;
        }

        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <returns></returns>
        public async Task<OmsOrderDto> GetAsync(Guid orderId)
        {
            var order = await _manager.GetAsync(orderId);
            return _mapper.Map<OmsOrderDto>(order);
        }

        /// <summary>
        /// 查询指定账号订单列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="userName">购买账号</param>
        /// <param name="state">订单状态</param>
        /// <param name="payState">支付状态（默认显示待支付）</param>
        /// <returns></returns>]
        public async Task<PageList<OmsOrderDto>> GetPgaeAsync(
            int pageIndex,
            int pageSize,
            string userName,
            OmsOrderStateEnum? state,
            OmsOrderPayStateEnum? payState)
        {
            var data = await _manager.GetPgaeAsync(pageIndex, pageSize, userName, state, payState);
            var items = _mapper.Map<IEnumerable<OmsOrderAggr>, IEnumerable<OmsOrderDto>>(data.Items);
            return new PageList<OmsOrderDto>(data.Total, data.PageIndex, data.PageSize, items);
        }
    }
}

