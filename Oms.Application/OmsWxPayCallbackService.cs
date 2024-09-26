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
    public class OmsWxPayCallbackService : IOmsWxPayCallbackService
    {
        private readonly IOmsOrderLogManager _logManager;
        private readonly IOmsWxmpPayCallbackManager _manager;
        private readonly IOmsOrderCallbackRecordManager _cbManager;
        private readonly IOmsWxPaySettingRepository _settingRepository;

        public OmsWxPayCallbackService(
            IOmsOrderLogManager logManager,
            IOmsWxmpPayCallbackManager manager,
            IOmsOrderCallbackRecordManager cbManager,
            IOmsWxPaySettingRepository settingRepository)
        {
            _manager = manager;
            _cbManager = cbManager;
            _logManager = logManager;
            _settingRepository = settingRepository;
        }

        /// <summary>
        /// 回调更新订单状态
        /// </summary>
        /// <param name="settingId">商户设置id</param>
        /// <param name="form">支付回调</param>
        /// <returns></returns>
        public async Task<OmsWxPayCallbackDto> UpdateOrderAsync(Guid settingId, OmsWxmpPayCallbackForm form)
        {
            var setting = await _settingRepository.GetIQFAsync(settingId);
            if (setting == null)
                return new OmsWxPayCallbackDto() { Code = "FAIL", Message = "商户数据异常" };

            // 1. 解密微信密文
            var content = AesGcmHelper.Decrypt(form.Resource.AssociatedData, form.Resource.Nonce, form.Resource.Ciphertext, setting.APIv3Key);
            var wxOrder = content.FromJson<OmsWxmpPayCallbackOrderForm>();

            // 2. 更新订单信息
            var result = await _manager.UpdateOrderAsync(setting, wxOrder);

            // 3. 回调业务系统
            if (result.ErrType == BaseErrType.Success)
            {
                var order = result.Data as OmsOrder;
                await _logManager.AddAsync(new OmsOrderLogForm()
                {
                    OrderId = order.Id,
                    State = order.State,
                    PayState = order.PayState,
                    ShippingState = order.ShippingState
                });

                if (!setting.CallbackUrl.IsNullOrEmpty())
                    await _cbManager.SynOrderAsync(setting.CallbackUrl, order.Id);
            }

            return GetCallBackData(result.ErrType);
        }

        private OmsWxPayCallbackDto GetCallBackData(BaseErrType errType)
        {
            switch (errType)
            {
                case BaseErrType.Success:
                    return new OmsWxPayCallbackDto();
                case BaseErrType.NotAllow:
                    return new OmsWxPayCallbackDto() { Code = "FAIL", Message = "未安装证书" };
                case BaseErrType.DataNotFound:
                    return new OmsWxPayCallbackDto() { Code = "FAIL", Message = "订单不存在" };
                case BaseErrType.AuthCodeInvalid:
                    return new OmsWxPayCallbackDto() { Code = "FAIL", Message = "签名错误" };
                case BaseErrType.DataError:
                    return new OmsWxPayCallbackDto() { Code = "FAIL", Message = "商户数据异常" };
                default:
                    return new OmsWxPayCallbackDto() { Code = "FAIL", Message = "未知错误" };
            }
        }
    }
}
