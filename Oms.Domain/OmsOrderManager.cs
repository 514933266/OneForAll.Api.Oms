using AutoMapper;
using Oms.Domain.Interfaces;
using Oms.Domain.Models;
using Oms.Domain.Repositorys;
using OneForAll.Core.DDD;
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

namespace Oms.Domain
{
    /// <summary>
    /// 订单管理
    /// </summary>
    public class OmsOrderManager : OmsBaseManager, IOmsOrderManager
    {
        private readonly string CACHE_KEY = "Order:{0}";
        private readonly IOmsOrderRepository _repository;
        private readonly IDistributedCache _cacheRepository;
        public OmsOrderManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IOmsOrderRepository repository,
            IDistributedCache cacheRepository) : base(mapper, httpContextAccessor)
        {
            _repository = repository;
            _cacheRepository = cacheRepository;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>调起微信支付的sign</returns>
        public async Task<BaseMessage> CreateAsync(OmsOrderCreateForm form)
        {
            var msg = new BaseMessage();
            // 后续订单量上来，可将此处替换为雪花算法
            var orderNo = DateTime.Now.ToString("yyyyMMddhhmmss") + StringHelper.GetRandomNumber(6);
            var data = _mapper.Map<OmsOrder>(form);
            data.OrderNo = orderNo.TryLong();
            try
            {
                var cacheKey = CACHE_KEY.Fmt($"{LoginUser.Id}{form.ProductName}".ToMd5());
                var no = await _cacheRepository.GetStringAsync(cacheKey);
                if (!no.IsNullOrEmpty())
                {
                    // 订单已创建，但未支付
                    msg.ErrType = BaseErrType.DataExist;
                    msg.Data = no;
                }
                else
                {
                    await _cacheRepository.SetStringAsync(cacheKey, orderNo);
                    var effected = await _repository.AddAsync(data);
                    if (effected > 0)
                    {
                        msg.Data = orderNo;
                        msg.ErrType = BaseErrType.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                // redis服务没有启动，发送日志
                throw new Exception("redis服务异常：" + ex.StackTrace);
            }

            return msg;
        }
    }
}
