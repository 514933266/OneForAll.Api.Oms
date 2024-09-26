using AutoMapper;
using Microsoft.AspNetCore.Http;
using Oms.Domain.AggregateRoots;
using Oms.Domain.Interfaces;
using Oms.Domain.Models;
using Oms.Domain.Repositorys;
using OneForAll.Core;
using OneForAll.Core.Extension;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OneForAll.Core.Security;
using Microsoft.Extensions.Caching.Distributed;
using Oms.Domain.Aggregates;

namespace Oms.Domain
{
    /// <summary>
    /// 微信小程序支付回调
    /// </summary>
    public class OmsWxmpPayCallbackManager : OmsBaseManager, IOmsWxmpPayCallbackManager
    {
        private readonly string CACHE_KEY = "Order:{0}";
        private readonly IDistributedCache _cacheRepository;
        private readonly IOmsOrderRepository _orderRepository;
        private readonly IOmsWxPaySettingRepository _settingRepository;
        public OmsWxmpPayCallbackManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IDistributedCache cacheRepository,
            IOmsOrderRepository orderRepository,
            IOmsWxPaySettingRepository settingRepository) : base(mapper, httpContextAccessor)
        {
            _cacheRepository = cacheRepository;
            _orderRepository = orderRepository;
            _settingRepository = settingRepository; ;
        }

        /// <summary>
        /// 更新订单
        /// </summary>
        /// <param name="setting">商户设置</param>
        /// <param name="wxOrder">微信订单数据</param>
        /// <returns></returns>
        public async Task<BaseMessage> UpdateOrderAsync(OmsWxPaySetting setting, OmsWxmpPayCallbackOrderForm wxOrder)
        {
            var result = new BaseMessage();
            // 1. 校验签名，不校验将存在致命风险，可能会影响账户金额
            var certPath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + setting.CertificateUrl);

            if (!File.Exists(certPath))
            {
                if (setting.Error.IsNullOrEmpty())
                {
                    setting.Error = "检测到商户未上传微信支付证书，请尽快上传避免产生损失";
                    await _settingRepository.SaveChangesAsync();
                }
                return result.Fail(BaseErrType.NotAllow, setting.Error);
            }

            // 2. 更新订单
            var arr = wxOrder.Attach.Split('|');
            if (arr.Length < 2)
                return result.Fail(BaseErrType.DataError, "订单Attach参数未设置");

            var orderId = new Guid(arr[0]);
            var userId = new Guid(arr[1]);
            var order = await _orderRepository.GetIQFAsync(orderId);
            if (order == null)
                return result.Fail(BaseErrType.DataNotFound, "订单不存在");

            order.Source = setting.AppName;
            order.UpdateTime = DateTime.Now;
            order.SynWxOrder(wxOrder);
            result.Data = order;

            var errType = await ResultAsync(_orderRepository.SaveChangesAsync);

            // 3. 清除缓存
            var cacheKey = CACHE_KEY.Fmt($"{userId}{order.ProductName}".ToMd5());
            var cacheNo = await _cacheRepository.GetStringAsync(cacheKey);
            if (!cacheNo.IsNullOrEmpty())
            {
                await _cacheRepository.RemoveAsync(cacheKey);
            }
            return errType == BaseErrType.Success ? result.Success() : result.Fail();
        }
    }
}
