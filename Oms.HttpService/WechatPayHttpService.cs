using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Oms.HttpService.Interfaces;
using OneForAll.Core.Extension;
using System.Net.Http.Formatting;
using Oms.HttpService.Models;
using static System.Collections.Specialized.BitVector32;
using System.Security.Cryptography;

namespace Oms.HttpService
{
    /// <summary>
    /// 微信小程序
    /// </summary>
    public class WechatPayHttpService : BaseHttpService, IWechatPayHttpService
    {
        private readonly HttpServiceConfig _config;

        public WechatPayHttpService(
            HttpServiceConfig config,
            IHttpContextAccessor httpContext,
            IHttpClientFactory httpClientFactory) : base(httpContext, httpClientFactory)
        {
            _config = config;
        }

        /// <summary>
        /// 预创建订单
        /// </summary>
        /// <param name="request">实体</param>
        /// <returns>微信的payid，用于调起支付</returns>
        public async Task<string> CreateOrderAsync(WxJSAPIOrderRequest request)
        {
            var prepayId = string.Empty;
            var client = GetHttpClient(_config.WxJSAPIOrder);
            if (client != null && client.BaseAddress != null)
            {
                var response = await client.PostAsync(client.BaseAddress, request, new JsonMediaTypeFormatter());
                var result = await response.Content.ReadAsAsync<WxJSAPIOrderResponse>();
                prepayId = result.PrepayId;
            }
            return prepayId;
        }
    }
}
