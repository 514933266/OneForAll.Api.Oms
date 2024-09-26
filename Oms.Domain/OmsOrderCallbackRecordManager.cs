using AutoMapper;
using Oms.Domain.Interfaces;
using Oms.Domain.Models;
using Oms.Domain.Repositorys;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oms.Domain.AggregateRoots;
using Microsoft.AspNetCore.Http;
using Oms.HttpService.Interfaces;
using Oms.Domain.Aggregates;

namespace Oms.Domain
{
    /// <summary>
    /// 订单回调记录
    /// </summary>
    public class OmsOrderCallbackRecordManager : OmsBaseManager, IOmsOrderCallbackRecordManager
    {
        private readonly IOmsOrderRepository _orderRepository;
        private readonly IOmsOrderItemRepository _itemRepository;
        private readonly IOmsOrderCallbackRecordRepository _repository;
        private readonly IOrderCallbackHttpService _httpService;
        public OmsOrderCallbackRecordManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IOmsOrderRepository orderRepository,
            IOmsOrderItemRepository itemRepository,
            IOmsOrderCallbackRecordRepository repository,
            IOrderCallbackHttpService httpService) : base(mapper, httpContextAccessor)
        {
            _orderRepository = orderRepository;
            _itemRepository = itemRepository;
            _repository = repository;
            _httpService = httpService;
        }

        /// <summary>
        /// 创建回调记录
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>调起微信支付的sign</returns>
        public async Task<BaseErrType> AddAsync(OmsOrderCallbackRecord form)
        {
            var exists = await _repository.GetAsync(w => w.OmsOrderId == form.OmsOrderId);
            if (exists != null)
                return BaseErrType.Success;

            return await ResultAsync(() => _repository.AddAsync(form));
        }

        /// <summary>
        /// 更新回调成功信息
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>调起微信支付的sign</returns>
        public async Task<BaseErrType> UpdateAsync(OmsOrderCallbackRecordUpdateForm form)
        {
            var data = await _repository.GetAsync(w => w.OmsOrderId == form.OrderId);
            if (data == null)
                return BaseErrType.DataNotFound;

            data.Error = form.Error;
            if (form.IsSuccess)
            {
                data.IsSuccess = form.IsSuccess;
                data.SuccessTime = DateTime.Now;
            }
            data.SetTimeStep();
            data.LastUpdateTime = DateTime.Now;

            return await ResultAsync(_repository.SaveChangesAsync);
        }

        /// <summary>
        /// 回传订单信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="orderId">订单id</param>
        /// <returns></returns>
        public async Task<BaseMessage> SynOrderAsync(string url, Guid orderId)
        {
            var result = new BaseMessage();
            var order = await _orderRepository.GetIQFAsync(orderId);
            if (order != null)
            {
                var items = await _itemRepository.GetListAsync(w => w.OmsOrderId == orderId);
                var data = _mapper.Map<OmsOrder, OmsOrderCallBackAggr>(order);
                if (items.Any())
                    data.Items.AddRange(items);
                result = await _httpService.SynOrderAsync(url, data);
                result.Data = order;
                result.ErrType = await UpdateAsync(new OmsOrderCallbackRecordUpdateForm()
                {
                    OrderId = orderId,
                    Error = result.Message,
                    IsSuccess = result.Status
                });
            }
            return result;
        }

        /// <summary>
        /// 回传订单信息
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <returns></returns>
        public async Task<BaseMessage> SynOrderAsync(Guid orderId)
        {
            var result = new BaseMessage();
            var data = await _repository.GetAsync(w => w.OmsOrderId == orderId);
            if (data != null)
            {
                result = await SynOrderAsync(data.CallBackUrl, orderId);
            }
            return result;
        }
    }
}
