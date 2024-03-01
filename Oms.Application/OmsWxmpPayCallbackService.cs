using AutoMapper;
using NPOI.OpenXml4Net.OPC.Internal;
using Oms.Application.Dtos;
using Oms.Application.Interfaces;
using Oms.Domain.AggregateRoots;
using Oms.Domain.Interfaces;
using Oms.Domain.Models;
using Oms.Domain.Repositorys;
using OneForAll.Core;
using OneForAll.Core.Extension;
using OneForAll.Core.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Application
{
    /// <summary>
    /// 微信小程序支付回调
    /// </summary>
    public class OmsWxmpPayCallbackService : IOmsWxmpPayCallbackService
    {
        private readonly IOmsOrderLogManager _logManager;
        private readonly IOmsWxmpPayCallbackManager _manager;
        private readonly IOmsWxPaySettingRepository _settingRepository;

        public OmsWxmpPayCallbackService(
            IOmsOrderLogManager logManager,
            IOmsWxmpPayCallbackManager manager,
            IOmsWxPaySettingRepository settingRepository)
        {
            _manager = manager;
            _logManager = logManager;
            _settingRepository = settingRepository;
        }

        /// <summary>
        /// 回调更新订单状态
        /// </summary>
        /// <param name="settingId">商户设置id</param>
        /// <param name="form">支付回调</param>
        /// <returns></returns>
        public async Task<OmsWxmpPayCallbackDto> UpdateOrderAsync(Guid settingId, OmsWxmpPayCallbackForm form)
        {
            var setting = await _settingRepository.GetIQFAsync(settingId);
            if (setting == null)
                return new OmsWxmpPayCallbackDto() { Code = "FAIL", Message = "商户数据异常" };

            // 1. 解密微信密文
            var content = AesGcmHelper.Decrypt(form.Resource.AssociatedData, form.Resource.Nonce, form.Resource.Ciphertext, setting.APIv3Key);
            var wxOrder = content.FromJson<OmsWxmpPayCallbackOrderForm>();

            // 2. 更新订单信息
            if (wxOrder.Attach.IsNullOrEmpty())
                return GetCallBackData(BaseErrType.DataNotFound);

            var errType = await _manager.UpdateOrderAsync(setting, wxOrder);

            // 3. 记录支付日志
            var order = new OmsOrder();
            order.SynWxOrder(wxOrder);
            await _logManager.AddAsync(new OmsOrderLogForm()
            {
                OrderId = order.Id,
                State = order.State,
                PayState = order.PayState,
                ShippingState = order.ShippingState
            });

            return GetCallBackData(errType);
        }

        private OmsWxmpPayCallbackDto GetCallBackData(BaseErrType errType)
        {
            switch (errType)
            {
                case BaseErrType.Success:
                    return new OmsWxmpPayCallbackDto();
                case BaseErrType.NotAllow:
                    return new OmsWxmpPayCallbackDto() { Code = "FAIL", Message = "未安装证书" };
                case BaseErrType.DataNotFound:
                    return new OmsWxmpPayCallbackDto() { Code = "FAIL", Message = "订单不存在" };
                case BaseErrType.AuthCodeInvalid:
                    return new OmsWxmpPayCallbackDto() { Code = "FAIL", Message = "签名错误" };
                case BaseErrType.DataError:
                    return new OmsWxmpPayCallbackDto() { Code = "FAIL", Message = "商户数据异常" };
                default:
                    return new OmsWxmpPayCallbackDto() { Code = "FAIL", Message = "未知错误" };
            }
        }
    }
}
