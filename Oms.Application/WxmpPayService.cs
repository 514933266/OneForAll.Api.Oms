using AutoMapper;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.Record.Chart;
using Oms.Application.Dtos;
using Oms.Application.Interfaces;
using Oms.Domain.AggregateRoots;
using Oms.Domain.Interfaces;
using Oms.Domain.Models;
using Oms.Domain.Repositorys;
using Oms.HttpService.Interfaces;
using Oms.HttpService.Models;
using Oms.Public;
using Oms.Public.Models;
using OneForAll.Core;
using OneForAll.Core.Extension;
using OneForAll.Core.Security;
using OneForAll.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Application
{
    /// <summary>
    /// 微信小程序支付
    /// </summary>
    public class WxmpPayService : IWxmpPayService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IOmsOrderManager _orderManager;
        private readonly IOmsWxPaySettingRepository _wxSettingRepository;
        private readonly IWechatPayHttpService _wxPayHttpService;
        public WxmpPayService(
            IMapper mapper,
            IHttpContextAccessor httpContext,
            IOmsOrderManager orderManager,
            IOmsWxPaySettingRepository wxSettingRepository,
            IWechatPayHttpService wxPayHttpService)
        {
            _mapper = mapper;
            _httpContext = httpContext;
            _orderManager = orderManager;
            _wxSettingRepository = wxSettingRepository;
            _wxPayHttpService = wxPayHttpService;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="user">登录用户</param>
        /// <param name="form">表单</param>
        /// <returns>微信小程序调起JSAPI支付参数</returns>
        public async Task<WxmpPayJSAPIDto> CreateOrderAsync(LoginUser user, OmsOrderCreateForm form)
        {
            var prepayId = string.Empty;
            var setting = await _wxSettingRepository.GetAsync(w => w.SysTenantId == user.SysTenantId);
            var msg = await _orderManager.CreateAsync(form);

            var order = msg.GetData<OmsOrder>();
            if (msg.ErrType == BaseErrType.Success)
            {
                prepayId = await _wxPayHttpService.CreateOrderAsync(new WxJSAPIOrderRequest()
                {
                    Attach = order.OrderNo.ToString(),
                    AppId = user.WxAppId,
                    Mchid = setting.Mchid,
                    Description = form.ProductName,
                    TimeExpire = DateTime.Now.AddMinutes(setting.OrderTimeExpire).ToString(),
                    Payer = new WxJSAPIOrderPayerRequest() { Openid = user.WxOpenId },
                    Amount = new WxJSAPIOrderAmountRequest() { Total = (form.TotalPrice * 100).TryInt() },
                    NotifyUrl = _httpContext.HttpContext.Request.Host + "/open-api/WxmpPayCallback"
                });
            }
            else if (msg.ErrType == BaseErrType.DataExist)
            {
                prepayId = order.PlatformOrderNo;
            }

            if (!prepayId.IsNullOrEmpty())
            {
                var tt = TimeHelper.ToTimeStamp().ToString();
                var nonceStr = StringHelper.GetRandomString(6);
                var package = "prepay_id=" + prepayId;
                var paySign = Sha256HashHelper.Encrypt($"{user.WxAppId}{tt}{nonceStr}{package}").ToBase64();
                return new WxmpPayJSAPIDto()
                {
                    TimeStamp = tt,
                    NonceStr = nonceStr,
                    Package = package,
                    PaySign = paySign
                };
            }
            return null;
        }
    }
}
