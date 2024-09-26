using Microsoft.AspNetCore.Http;
using Oms.HttpService.Interfaces;
using Oms.HttpService.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace Oms.HttpService
{
    /// <summary>
    /// 订单支付回调
    /// </summary>
    public class OrderCallbackHttpService : BaseHttpService, IOrderCallbackHttpService
    {

        public OrderCallbackHttpService(IHttpContextAccessor httpContext, IHttpClientFactory httpClientFactory) : base(httpContext, httpClientFactory)
        {
        }

        /// <summary>
        /// 回传订单信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="order">订单</param>
        /// <returns></returns>
        public async Task<BaseMessage> SynOrderAsync(string url, object order)
        {
            var result = new BaseMessage();
            try
            {
                var client = new HttpClient();
                var response = await client.PostAsync(url, order, new JsonMediaTypeFormatter());
                result = await response.Content.ReadAsAsync<BaseMessage>();
            }
            catch (Exception ex)
            {
                result.Message = "请求异常：" + ex.Message;
            }
            return result;
        }
    }
}
