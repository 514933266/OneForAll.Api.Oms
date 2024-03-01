using AutoMapper;
using Microsoft.AspNetCore.Http;
using Oms.Domain.AggregateRoots;
using Oms.Domain.Interfaces;
using Oms.Domain.Models;
using Oms.Domain.Repositorys;
using Oms.Public;
using OneForAll.Core;
using OneForAll.Core.Extension;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OneForAll.EFCore;
using OneForAll.Core.Security;
using Microsoft.Extensions.Caching.Distributed;
using Oms.Domain.Enums;
using Oms.Domain.Aggregates;
using Oms.HttpService.Models;
using Oms.HttpService.Interfaces;
using Oms.Public.Models;

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
        private readonly IOmsOrderItemRepository _itemRepository;
        private readonly IOmsWxPaySettingRepository _settingRepository;
        private readonly IOmsOrderCallbackRecordRepository _callbackRepository;
        public OmsWxmpPayCallbackManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IDistributedCache cacheRepository,
            IOmsOrderRepository orderRepository,
            IOmsOrderItemRepository itemRepository,
            IOmsWxPaySettingRepository settingRepository,
            IOmsOrderCallbackRecordRepository callbackRepository) : base(mapper, httpContextAccessor)
        {
            _cacheRepository = cacheRepository;
            _orderRepository = orderRepository;
            _settingRepository = settingRepository; ;
            _callbackRepository = callbackRepository;
            _itemRepository = itemRepository;
        }

        /// <summary>
        /// 更新订单
        /// </summary>
        /// <param name="setting">商户设置</param>
        /// <param name="wxOrder">微信订单数据</param>
        /// <returns></returns>
        public async Task<BaseErrType> UpdateOrderAsync(OmsWxPaySetting setting, OmsWxmpPayCallbackOrderForm wxOrder)
        {
            // 1. 校验签名，不校验将存在致命风险，可能会影响账户金额
            var certPath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + setting.CertificateUrl);

            if (!File.Exists(certPath))
            {
                if (setting.Error.IsNullOrEmpty())
                {
                    setting.Error = "检测商户已接入支付，但未上传微信支付证书，请尽快上传避免产生损失";
                    await _settingRepository.SaveChangesAsync();
                }
                return BaseErrType.NotAllow;
            }

            // 2. 更新订单
            var arr = wxOrder.Attach.Split('|');
            if (arr.Length < 2)
                return BaseErrType.DataError;

            var orderId = new Guid(arr[0]);
            var userId = new Guid(arr[1]);
            var order = await _orderRepository.GetIQFAsync(orderId);
            if (order == null)
                return BaseErrType.DataNotFound;

            order.Source = setting.AppName;
            order.UpdateTime = DateTime.Now;
            order.SynWxOrder(wxOrder);

            var errType = await ResultAsync(_orderRepository.SaveChangesAsync);
            if (errType == BaseErrType.Success && !setting.CallbackUrl.IsNullOrEmpty())
                errType = await SynOrderAsync(setting.CallbackUrl, order);

            // 3. 清除缓存
            var cacheKey = CACHE_KEY.Fmt($"{userId}{order.ProductName}".ToMd5());
            var cacheNo = await _cacheRepository.GetStringAsync(cacheKey);
            if (!cacheNo.IsNullOrEmpty())
            {
                await _cacheRepository.RemoveAsync(cacheKey);
            }
            return errType;
        }

        /// <summary>
        /// 回传订单信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="order">订单</param>
        /// <returns></returns>
        private async Task<BaseErrType> SynOrderAsync(string url, OmsOrder order)
        {
            /* 此版本忽略签名校验以及确认返回流程，后续补上 */
            try
            {
                var orderRequest = _mapper.Map<OmsOrderCallBackAggr>(order);
                var items = await _itemRepository.GetListAsync(w => w.OmsOrderId == order.Id);
                if (items.Any())
                {
                    orderRequest.Items.AddRange(items);
                }

                var client = new HttpClient();
                var response = await client.PostAsync(url, orderRequest, new JsonMediaTypeFormatter());
                var result = await response.Content.ReadAsAsync<BaseMessage>();
                var record = await _callbackRepository.GetAsync(w => w.OmsOrderId == order.Id);
                if (record != null)
                {
                    record.Error = result.Message;
                    record.LastUpdateTime = DateTime.Now;
                    record.IsSuccess = result.Status;
                    record.SuccessTime = result.Status ? DateTime.Now : default;
                    record.LastUpdateTime = DateTime.Now;

                    return await ResultAsync(() => _callbackRepository.UpdateAsync(record));
                }
            }
            catch
            {
                // 回传失败，待定时任务MonitorOrderCallbackJob重新回传
                return BaseErrType.DataError;
            }
            return BaseErrType.DataNotFound;
        }
    }
}
