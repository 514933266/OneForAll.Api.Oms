using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Oms.Domain.AggregateRoots;
using Oms.Domain.Interfaces;
using Oms.Domain.Repositorys;
using Oms.HttpService.Interfaces;
using Oms.HttpService.Models;
using Oms.Public;
using Oms.Public.Models;
using OneForAll.Core;
using OneForAll.Core.Extension;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain
{
    /// <summary>
    /// 微信订单
    /// </summary>
    public class OmsWxmpOrderManager : OmsBaseManager, IOmsWxOrderManager
    {
        private readonly AuthConfig _app;
        private readonly IConfiguration _config;
        private readonly IWechatPayHttpService _wxPayHttpService;
        private readonly IOmsWxPaySettingRepository _paySettingRepository;
        private readonly ISysGlobalExceptionLogHttpService _logHttpService;

        public OmsWxmpOrderManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            AuthConfig app,
            IConfiguration config,
            IWechatPayHttpService wxPayHttpService,
            IOmsWxPaySettingRepository paySettingRepository,
            ISysGlobalExceptionLogHttpService logHttpService) : base(mapper, httpContextAccessor)
        {
            _app = app;
            _config = config;
            _wxPayHttpService = wxPayHttpService;
            _paySettingRepository = paySettingRepository;
            _logHttpService = logHttpService;
        }

        /// <summary>
        /// 创建小程序支付订单
        /// </summary>
        /// <param name="setting">商户支付设置</param>
        /// <param name="order">系统订单</param>
        /// <returns>调起微信支付的sign</returns>
        public async Task<WxmpPayData> CreateWxmpAsync(OmsWxPaySetting setting, OmsOrder order)
        {
            if (order == null) throw new Exception("订单数据丢失");
            if (setting == null) throw new Exception("商户设置丢失");
            if (order.PlatformPayerId.IsNullOrEmpty()) throw new Exception("用户支付数据丢失");

            var keyPath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + setting.CertificateKeyUrl);
            var certPath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + setting.CertificateUrl);
            var privateKey = WxmpPayV3SDK.GetCertPrivateKey(keyPath);
            if (setting.CertSerialNo.IsNullOrEmpty())
            {
                // 商户没有填写证书序列号，但是有上传拿证书时自动读取序列号并更新
                var certSerialNo = CertificateHelper.GetSerialNumber(certPath);
                if (!certSerialNo.IsNullOrEmpty())
                {
                    setting.CertSerialNo = certSerialNo;
                    await _paySettingRepository.UpdateAsync(setting);
                }
            }
            if (order.PlatformOrderNo.IsNullOrEmpty())
            {
                var url = _config["WxmpPay:NotifyUrl"];
                var amount = (int)(order.TotalPrice * 100);
                var msg = await _wxPayHttpService.CreateWxmpOrderAsync(setting.Mchid, setting.CertSerialNo, privateKey, new WxmpOrderRequest()
                {
                    AppId = setting.AppId,
                    Mchid = setting.Mchid,
                    // 将订单id和用户id作为参数用于回调处理
                    Attach = $"{order.Id}|{LoginUser.Id}",
                    OutTradeNo = order.OrderNo.ToString(),
                    Description = order.ProductName,
                    TimeExpire = DateTime.Now.AddMinutes(setting.OrderTimeExpire).ToString("yyyy-MM-ddTHH:mm:ssK"),
                    Payer = new WxJSAPIOrderPayerRequest() { Openid = order.PlatformPayerId },
                    Amount = new WxJSAPIOrderAmountRequest() { Total = amount },
                    // 回调Url中将商户的配置id带过去用于解密微信的密文
                    NotifyUrl = $"{url}/{setting.Id}"
                });
                if (msg.Status)
                {
                    order.PlatformOrderNo = msg.Data.ToString();
                }
                else
                {
                    if (msg.ErrType == BaseErrType.Overflow)
                        return new WxmpPayData() { NonceStr = "订单已支付" };
                    return null;
                }
            }

            return WxmpPayV3SDK.GetWxmpPayData(setting.AppId, order.PlatformOrderNo, privateKey);
        }
    }
}
