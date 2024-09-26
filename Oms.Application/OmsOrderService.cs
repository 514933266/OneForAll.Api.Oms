using AutoMapper;
using Oms.Application.Dtos;
using Oms.Application.Interfaces;
using Oms.Domain.Aggregates;
using Oms.Domain.Interfaces;
using Oms.Domain.Models;
using Oms.Domain.Repositorys;
using Oms.Domain.ValueObject;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Application
{
    /// <summary>
    /// 订单
    /// </summary>
    public class OmsOrderService : IOmsOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOmsOrderManager _manager;
        private readonly IOmsOrderRepository _repository;

        public OmsOrderService(
            IMapper mapper,
            IOmsOrderManager manager,
            IOmsOrderRepository repository)
        {
            _mapper = mapper;
            _manager = manager;
            _repository = repository;
        }

        /// <summary>
        /// 查询指定日期的订单当日金额统计
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public async Task<OmsOrderTodayAmountVo> GetTodayAmountAsync(DateTime date)
        {
           return await _repository.CountTodayAmountAsync(date);
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
        /// 查询订单列表
        /// </summary>
        /// <param name="orderNos">订单编号</param>
        /// <returns></returns>
        public async Task<IEnumerable<OmsOrderDto>> GetListAsync(List<string> orderNos)
        {
            var data = await _manager.GetListAsync(orderNos);
            return _mapper.Map<IEnumerable<OmsOrderAggr>, IEnumerable<OmsOrderDto>>(data);
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="form">参数表单</param>
        /// <returns>分页列表</returns>
        public async Task<PageList<OmsOrderDto>> GetPageAsync(int pageIndex, int pageSize, OmsGetPageOrderForm form)
        {
            var data = await _manager.GetPageAsync(pageIndex, pageSize, form);
            var items = _mapper.Map<IEnumerable<OmsOrderAggr>, IEnumerable<OmsOrderDto>>(data.Items);
            return new PageList<OmsOrderDto>(data.Total, data.PageSize, data.PageIndex, items);
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>创建结果</returns>
        public async Task<BaseErrType> AddAsync(OmsCustomOrderForm form)
        {
            return await _manager.AddAsync(form);
        }
    }
}

