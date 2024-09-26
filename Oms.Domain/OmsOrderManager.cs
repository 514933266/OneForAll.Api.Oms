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
using OneForAll.Core.Utility;
using Oms.Domain.AggregateRoots;
using OneForAll.Core.Extension;
using Microsoft.Extensions.Caching.Distributed;
using Oms.Public.Models;
using OneForAll.Core.Security;
using Microsoft.AspNetCore.Http;
using Oms.Domain.Aggregates;
using Oms.HttpService.Interfaces;
using Oms.HttpService.Models;
using OneForAll.EFCore;
using OneForAll.Core.ORM;
using Oms.Domain.Enums;

namespace Oms.Domain
{
    /// <summary>
    /// 订单管理
    /// </summary>
    public class OmsOrderManager : OmsBaseManager, IOmsOrderManager
    {
        private readonly AuthConfig _authConfig;
        private readonly string CACHE_KEY = "Order:{0}";
        private readonly IOmsOrderRepository _repository;
        private readonly IOmsOrderItemRepository _itemRepository;
        private readonly IDistributedCache _cacheRepository;
        public OmsOrderManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            AuthConfig authConfig,
            IOmsOrderRepository repository,
            IOmsOrderItemRepository itemRepository,
            IDistributedCache cacheRepository) : base(mapper, httpContextAccessor)
        {
            _authConfig = authConfig;
            _repository = repository;
            _itemRepository = itemRepository;
            _cacheRepository = cacheRepository;
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
                result.Items.AddRange(items);
            }
            return result;
        }

        /// <summary>
        /// 查询订单列表
        /// </summary>
        /// <param name="orderNos">订单编号</param>
        /// <returns></returns>
        public async Task<IEnumerable<OmsOrderAggr>> GetListAsync(List<string> orderNos)
        {
            var data = await _repository.GetListAsync(orderNos);
            var oids = data.Select(s => s.Id).ToList();
            var items = await _itemRepository.GetListAsync(w => oids.Contains(w.OmsOrderId));

            var orders = _mapper.Map<IEnumerable<OmsOrderAggr>>(data);
            orders.ForEach(e =>
            {
                var cItem = items.Where(w => w.OmsOrderId == e.OrderId).ToList();
                if (items.Any())
                    e.Items.AddRange(cItem);
            });
            return orders;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="form">参数表单</param>
        /// <returns>分页列表</returns>
        public async Task<PageList<OmsOrderAggr>> GetPageAsync(int pageIndex, int pageSize, OmsGetPageOrderForm form)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var data = await _repository.GetPageAsync(pageIndex, pageSize, form);
            var oids = data.Items.Select(s => s.Id).ToList();
            var items = await _itemRepository.GetListAsync(w => oids.Contains(w.OmsOrderId));

            var orders = _mapper.Map<IEnumerable<OmsOrderAggr>>(data.Items);
            var result = new PageList<OmsOrderAggr>(data.Total, data.PageIndex, data.PageSize, orders);
            result.Items.ForEach(e =>
            {
                var cItem = items.Where(w => w.OmsOrderId == e.OrderId).ToList();
                if (items.Any())
                    e.Items.AddRange(cItem);
            });
            return result;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>创建结果</returns>
        public async Task<BaseErrType> AddAsync(OmsCustomOrderForm form)
        {
            if (form.TenantId == Guid.Empty)
                form.TenantId = LoginUser.SysTenantId;
            var data = _mapper.Map<OmsCustomOrderForm, OmsOrder>(form);
            data.CreateOrderNo();

            var items = _mapper.Map<IEnumerable<OmsOrderItem>>(form.Items);
            using (var tran = new UnitOfWork().BeginTransaction())
            {
                await _repository.AddAsync(data, tran);
                items.ForEach(e => e.OmsOrderId = data.Id);
                await _itemRepository.AddRangeAsync(items, tran);
                var errType = await ResultAsync(tran.CommitAsync);
                if (errType == BaseErrType.Success)
                    form.Id = data.Id;
                return errType;
            }
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="form">实体</param>
        /// <param name="setting">商户设置</param>
        /// <returns>调起微信支付的sign</returns>
        public async Task<OmsOrder> CreateAsync(OmsOrderForm form, OmsWxPaySetting setting)
        {
            OmsOrder data = null;
            if (form.Id != Guid.Empty)
            {
                data = await _repository.FindAsync(form.Id);
            }
            else
            {
                var names = form.Items.Select(s => s.ProductNo).ToList();
                var nameStr = string.Join("-", names);
                var cacheKey = CACHE_KEY.Fmt($"{LoginUser.Id}{nameStr}".ToMd5());
                var cacheNo = await _cacheRepository.GetStringAsync(cacheKey);
                if (!cacheNo.IsNullOrEmpty())
                {
                    data = await _repository.FindAsync(new Guid(cacheNo));
                }

                if (data == null)
                {
                    data = await AddWxOrderAsync(form, setting.OrderTimeExpire);
                }
                else if (data != null && data.State != OmsOrderStateEnum.WaitingPay)
                {
                    // 订单已过期，但缓存未过期时创建新订单
                    data = await AddWxOrderAsync(form, setting.OrderTimeExpire);
                }
            }
            return data;
        }

        // 写入微信订单
        private async Task<OmsOrder> AddWxOrderAsync(OmsOrderForm form, int timeExpire)
        {
            var data = _mapper.Map<OmsOrderForm, OmsOrder>(form);
            data.UserId = LoginUser.Id.ToString();
            data.UserName = LoginUser.UserName ?? "";
            data.PlatformPayerId = LoginUser.WxOpenId ?? "";
            data.CreateOrderNo();

            if (timeExpire > 0)
            {
                data.MayFailureTime = DateTime.Now.AddMinutes(timeExpire);
            }

            var effected = 0;
            var items = _mapper.Map<IEnumerable<OmsOrderItem>>(form.Items);
            using (var tran = new UnitOfWork().BeginTransaction())
            {
                await _repository.AddAsync(data, tran);
                items.ForEach(e => e.OmsOrderId = data.Id);
                await _itemRepository.AddRangeAsync(items, tran);
                effected = await tran.CommitAsync();
            }

            if (effected > 0)
            {
                // 设置缓存，30分钟内可继续支付
                var cacheKey = CACHE_KEY.Fmt($"{LoginUser.Id}{form.ProductName}".ToMd5());
                await _cacheRepository.SetStringAsync(cacheKey, data.Id.ToString(), new DistributedCacheEntryOptions() { AbsoluteExpiration = DateTime.Now.AddMinutes(30) });
            }
            return data;
        }

        /// <summary>
        /// 更新第三方平台订单号
        /// </summary>
        /// <param name="id">订单id</param>
        /// <param name="orderNo">第三方平台订单号</param>
        /// <returns></returns>
        public async Task<BaseErrType> UpdatePlatformOrderNo(Guid id, string orderNo)
        {
            var data = await _repository.GetIQFAsync(id);
            if (data == null)
                return BaseErrType.DataNotFound;
            if (data.PlatformOrderNo != orderNo)
            {
                data.PlatformOrderNo = orderNo;
                return await ResultAsync(_repository.SaveChangesAsync);
            }
            else
            {
                // 强制更新
                return await ResultAsync(() => _repository.UpdateAsync(data));
            }
        }
    }
}
    