using AutoMapper;
using Microsoft.AspNetCore.Http;
using Oms.Application.Dtos;
using Oms.Application.Interfaces;
using Oms.Domain.AggregateRoots;
using Oms.Domain.Enums;
using Oms.Domain.Interfaces;
using Oms.Domain.Models;
using Oms.Domain.Repositorys;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Application
{
    /// <summary>
    /// 微信小程序支付
    /// </summary>
    public class OmsWxmpPayService : OmsBaseService, IOmsWxmpPayService
    {
        private readonly IOmsOrderManager _orderManager;
        private readonly IOmsWxOrderManager _wxOrderManager;
        private readonly IOmsOrderLogManager _logManager;
        private readonly IOmsOrderCallbackRecordManager _callbackManager;

        private readonly IOmsOrderRepository _orderRepository;
        private readonly IOmsWxPaySettingRepository _wxSettingRepository;


        public OmsWxmpPayService(
            IMapper mapper,
            IHttpContextAccessor httpContext,
            IOmsOrderManager orderManager,
            IOmsWxOrderManager wxOrderManager,
            IOmsOrderLogManager logManager,
            IOmsOrderCallbackRecordManager callbackManager,
            IOmsOrderRepository orderRepository,
            IOmsWxPaySettingRepository wxSettingRepository) : base(mapper, httpContext)
        {
            _logManager = logManager;
            _orderManager = orderManager;
            _wxOrderManager = wxOrderManager;
            _callbackManager = callbackManager;
            _orderRepository = orderRepository;
            _wxSettingRepository = wxSettingRepository;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>微信小程序调起JSAPI支付参数</returns>
        public async Task<BaseMessage> CreateOrderAsync(OmsOrderForm form)
        {
            var prepayId = string.Empty;
            var msg = new BaseMessage() { ErrType = BaseErrType.NotAllow };

            if (form.TenantId == Guid.Empty)
                form.TenantId = LoginUser.SysTenantId;
            var setting = await _wxSettingRepository.GetIQFAsync(form.TenantId, form.Mchid);
            if (setting == null || !setting.CheckCanRequestPay())
                return msg.Fail("未检测到商户支付设置");

            // 1. 创建系统订单
            var order = await _orderManager.CreateAsync(form, setting);
            if (order == null)
                return msg.Fail("订单异常");
            if (order.Id == Guid.Empty)
                return msg.Fail("创建订单失败");
            if (order.PayState == OmsOrderPayStateEnum.Paid)
                return msg.Fail("订单已支付");
            if (order.State == OmsOrderStateEnum.Canceled)
                return msg.Fail("订单已取消");
            if (order.PayState != OmsOrderPayStateEnum.UnPay)
                return msg.Fail("订单已结束");

            // 2. 请求商户填写的回调地址
            var callbackErrType = await _callbackManager.AddAsync(new OmsOrderCallbackRecord() { OmsOrderId = order.Id, CallBackUrl = setting.CallbackUrl });
            if (callbackErrType != BaseErrType.Success)
                return msg.Fail("商户回调数据写入失败，请稍后再试");

            // 3. 微信下单
            var payData = await _wxOrderManager.CreateWxmpAsync(setting, order);
            if (payData == null)
            {
                return msg.Fail("微信下单失败，请稍后再试");
            }
            else
            {
                if (payData.NonceStr == "订单已支付")
                {
                    order.Paid();
                    await _orderRepository.SaveChangesAsync();
                    await _logManager.AddAsync(new OmsOrderLogForm() { OrderId = order.Id, State = order.State, PayState = order.PayState, ShippingState = order.ShippingState, Remark = "微信返回已支付状态" });
                    return msg.Fail("订单已支付");
                }
            }

            // 4. 将微信的payid更新到订单号，未支付时可以重新用于发起支付
            var errType = await _orderManager.UpdatePlatformOrderNo(order.Id, order.PlatformOrderNo);
            if (errType != BaseErrType.Success)
            {
                msg.Data = new OmsWxmpCreateOrderDto() { OrderId = order.Id };
                return msg.Fail("请尝试重新发起支付");
            }

            // 5. 记录日志
            await _logManager.AddAsync(new OmsOrderLogForm() { OrderId = order.Id, State = OmsOrderStateEnum.WaitingPay, PayState = OmsOrderPayStateEnum.UnPay, ShippingState = OmsOrderShippingStateEnum.Pending });

            msg.Data = new OmsWxmpCreateOrderDto() { OrderId = order.Id, PayData = payData };
            return msg.Success("创建订单成功");
        }
    }
}
