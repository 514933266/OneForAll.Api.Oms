using AutoMapper;
using Oms.Domain.Interfaces;
using Oms.Domain.Models;
using Oms.Domain.Repositorys;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneForAll.Core.Utility;
using Oms.Domain.AggregateRoots;
using OneForAll.Core.Extension;
using Microsoft.Extensions.Caching.Distributed;
using Oms.Public.Models;
using OneForAll.Core.Security;
using Microsoft.AspNetCore.Http;
using Oms.Domain.Aggregates;
using Oms.HttpService.Interfaces;
using Oms.HttpService.Models;
using OneForAll.EFCore;

namespace Oms.Domain
{
    /// <summary>
    /// 订单回调记录
    /// </summary>
    public class OmsOrderCallbackRecordManager : OmsBaseManager, IOmsOrderCallbackRecordManager
    {
        private readonly IOmsOrderCallbackRecordRepository _repository;
        public OmsOrderCallbackRecordManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IOmsOrderCallbackRecordRepository repository) : base(mapper, httpContextAccessor)
        {
            _repository = repository;
        }

        /// <summary>
        /// 创建回调记录
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>调起微信支付的sign</returns>
        public async Task<BaseErrType> AddAsync(OmsOrderCallbackRecord form)
        {
            var exists = await _repository.GetAsync(w => w.OmsOrderId == form.OmsOrderId);
            if (exists != null)
                return BaseErrType.Success;
            if (form.CallBackUrl.IsNullOrEmpty())
                return BaseErrType.Success;

            return await ResultAsync(() => _repository.AddAsync(form));
        }

        /// <summary>
        /// 更新回调成功信息
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>调起微信支付的sign</returns>
        public async Task<BaseErrType> UpdateAsync(OmsOrderCallbackRecordUpdateForm form)
        {
            var data = await _repository.GetAsync(w => w.OmsOrderId == form.OrderId);
            if (data == null)
                return BaseErrType.DataNotFound;

            data.Error = form.Error;
            if (form.IsSuccess)
            {
                data.IsSuccess = form.IsSuccess;
                data.SuccessTime = DateTime.Now;
            }
            data.SetTimeStep();
            data.LastUpdateTime = DateTime.Now;

            return await ResultAsync(_repository.SaveChangesAsync);
        }
    }
}
