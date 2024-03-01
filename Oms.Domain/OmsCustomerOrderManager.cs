using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using NetTopologySuite.Operation.Overlay.Snap;
using NPOI.HSSF.Record.Chart;
using Oms.Domain.Aggregates;
using Oms.Domain.Enums;
using Oms.Domain.Interfaces;
using Oms.Domain.Repositorys;
using Oms.HttpService.Interfaces;
using Oms.Public.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain
{
    /// <summary>
    /// 个人订单
    /// </summary>
    public class OmsCustomerOrderManager : OmsBaseManager, IOmsPersonalOrderManager
    {
        private readonly IOmsOrderRepository _repository;
        private readonly IOmsOrderItemRepository _itemRepository;
        public OmsCustomerOrderManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IOmsOrderRepository repository,
            IOmsOrderItemRepository itemRepository) : base(mapper, httpContextAccessor)
        {
            _repository = repository;
            _itemRepository = itemRepository;
        }

        /// <summary>
        /// 查询订单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OmsOrderAggr> GetAsync(Guid id)
        {
            var data = await _repository.GetIQFAsync(id);
            if (data == null) return null;

            var items = await _itemRepository.GetListAsync(w => w.OmsOrderId == id);
            var result = _mapper.Map<OmsOrderAggr>(data);
            if (items.Any())
            {
                result.OmsOrderItems.AddRange(items);
            }
            return result;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="userName">购买账号</param>
        /// <param name="state">订单状态</param>
        /// <param name="payState">支付状态（默认显示待支付）</param>
        ///  <returns>分页</returns>
        public async Task<PageList<OmsOrderAggr>> GetPgaeAsync(
            int pageIndex,
            int pageSize,
            string userName,
            OmsOrderStateEnum? state,
            OmsOrderPayStateEnum? payState)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize > 100) pageSize = 100;

            var result = new List<OmsOrderAggr>();
            var data = await _repository.GetPagePersonalIQFAsync(pageIndex, pageSize, userName, state, payState);
            var ids = data.Items.Select(s => s.Id).ToList();
            if (data.Items.Count() > 0)
            {
                result = _mapper.Map<List<OmsOrderAggr>>(data.Items);
                var items = await _itemRepository.GetListAsync(w => ids.Contains(w.OmsOrderId));

                result.ForEach(e =>
                {
                    var curItems = items.Where(w => w.OmsOrderId == e.Id).OrderByDescending(o => o.Id).ToList();
                    if (curItems.Count > 0)
                        e.OmsOrderItems.AddRange(curItems);
                });
            }
            return new PageList<OmsOrderAggr>(data.Total, data.PageIndex, data.PageSize, result);
        }
    }
}
