using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Oms.HttpService.Interfaces;
using System.Net.Http.Formatting;
using Oms.HttpService.Models;
using Oms.Public;
using OneForAll.Core.Extension;
using OneForAll.Core;

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
        /// 预创建JSAPI订单
        /// </summary>
        /// <param name="request">实体</param>
        /// <returns>微信的payid，用于调起支付</returns>
        public async Task<BaseMessage> CreateJSAPIOrderAsync(WxJSAPIOrderRequest request)
        {
            var msg = new BaseMessage();
            var client = GetHttpClient(_config.WxJSAPIOrder);
            if (client != null && client.BaseAddress != null)
            {
                var response = await client.PostAsync(client.BaseAddress, request, new JsonMediaTypeFormatter());
                var str = await response.Content.ReadAsStringAsync();
                var result = str.FromJson<WxJSAPIOrderResponse>();
                if (result?.PrepayId == null)
                {
                    msg.Message = str;
                }
                else
                {
                    msg.Status = true;
                    msg.Data = result.PrepayId;
                }
            }
            return msg;
        }

        /// <summary>
        /// 预创建Wxmp订单
        /// </summary>
        /// <param name="mchid">商户id</param>
        /// <param name="serialNo">证书序列号</param>
        /// <param name="privateKey">api v3秘钥</param>
        /// <param name="request">实体</param>
        /// <returns>微信的payid，用于调起支付</returns>
        public async Task<BaseMessage> CreateWxmpOrderAsync(string mchid, string serialNo, string privateKey, WxmpOrderRequest request)
        {
            var msg = new BaseMessage();
            var client = GetHttpClientWithNoToken(_config.WxmpOrder);
            if (client != null && client.BaseAddress != null)
            {
                var sign = WxmpPayV3SDK.GetWxmpCreateOrderSign(mchid, serialNo, privateKey, request.ToJson());
                var userAgent = _httpContext.HttpContext.Request.Headers["User-Agent"].ToString();
                client.DefaultRequestHeaders.Add("Authorization", sign); ;
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", userAgent);

                var response = await client.PostAsync(client.BaseAddress, request, new JsonMediaTypeFormatter());
                var str = await response.Content.ReadAsStringAsync();
                var result = str.FromJson<WxmpOrderResponse>();
                if (result?.PrepayId == null)
                {
                    msg.Message = str;
                    if (!str.IsNullOrEmpty() && str.Contains("已支付") || str.Contains("重复支付"))
                        msg.ErrType = BaseErrType.Overflow;
                }
                else
                {
                    msg.Status = true;
                    msg.ErrType = BaseErrType.Success;
                    msg.Data = result.PrepayId;
                }
            }
            return msg;
        }
    }
}
