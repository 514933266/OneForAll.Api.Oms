using AutoMapper;
using Microsoft.AspNetCore.Http;
using Oms.Domain.AggregateRoots;
using Oms.Domain.Interfaces;
using Oms.Domain.Models;
using Oms.Domain.Repositorys;
using Oms.Domain.ValueObject;
using OneForAll.Core;
using OneForAll.Core.Extension;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain
{
    /// <summary>
    /// 订单日志
    /// </summary>
    public class OmsOrderLogManager : OmsBaseManager, IOmsOrderLogManager
    {
        private readonly IOmsOrderLogRepository _repository;
        public OmsOrderLogManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IOmsOrderLogRepository repository) : base(mapper, httpContextAccessor)
        {
            _repository = repository;
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="form">订单</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(OmsOrderLogForm form)
        {
            var exists = await _repository.GetAsync(w => w.OmsOrderId == form.OrderId);
            if (exists != null)
            {
                exists.AddDetail(form.State, form.PayState, form.Remark);
                return await ResultAsync(_repository.SaveChangesAsync);
            }
            else
            {
                var data = new OmsOrderLog() { OmsOrderId = form.OrderId };
                data.AddDetail(form.State, form.PayState, form.Remark);
                return await ResultAsync(() => _repository.AddAsync(data));
            }
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="forms">订单</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(IEnumerable<OmsOrderLogForm> forms)
        {
            if (forms.Count() < 0)
                return BaseErrType.DataEmpty;

            var addList = new List<OmsOrderLog>();
            var updList = new List<OmsOrderLog>();
            var ids = forms.Select(s => s.OrderId).ToList();
            var existsList = await _repository.GetListAsync(w => ids.Contains(w.OmsOrderId));
            forms.ForEach(e =>
            {
                var exists = existsList.FirstOrDefault(w => w.OmsOrderId == e.OrderId);
                if (exists != null)
                {
                    exists.AddDetail(e.State, e.PayState);
                    updList.Add(exists);
                }
                else
                {
                    var data = new OmsOrderLog() { OmsOrderId = e.OrderId };
                    data.AddDetail(e.State, e.PayState);
                    addList.Add(data);
                }
            });

            using (var tran = new UnitOfWork().BeginTransaction())
            {
                if (addList.Count > 0)
                    await _repository.AddRangeAsync(addList, tran);
                if (updList.Count > 0)
                    await _repository.UpdateRangeAsync(updList, tran);
                return await ResultAsync(tran.CommitAsync);
            };
        }
    }
}
