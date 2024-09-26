using Microsoft.EntityFrameworkCore;
using OneForAll.Core.ORM;
using OneForAll.Core;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oms.Domain.AggregateRoots;
using Oms.Domain.Repositorys;
using Oms.Domain.Enums;
using OneForAll.Core.Extension;
using Oms.Domain.Models;
using Oms.Domain.ValueObject;
using Oms.Domain.Aggregates;

namespace Oms.Repository
{
    /// <summary>
    /// 订单
    /// </summary>
    public class OmsOrderRepository : Repository<OmsOrder>, IOmsOrderRepository
    {
        public OmsOrderRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 查询指定订单
        /// </summary>
        /// <returns>实体</returns>
        public async Task<OmsOrder> GetIQFAsync(Guid id)
        {
            return await DbSet.IgnoreQueryFilters().FirstOrDefaultAsync(w => w.Id == id);
        }

        /// <summary>
        /// 查询指定订单
        /// </summary>
        /// <param name="orderNo">订单编号</param>
        /// <returns>实体</returns>
        public async Task<OmsOrder> GetIQFAsync(string orderNo)
        {
            var no = orderNo.TryLong();
            return await DbSet.IgnoreQueryFilters().FirstOrDefaultAsync(w => w.OrderNo == no);
        }

        /// <summary>
        /// 查询指定订单
        /// </summary>
        /// <returns>实体</returns>
        public async Task<OmsOrderAggr> GetAggrIQFAsync(Guid id)
        {
            var itemDbSet = Context.Set<OmsOrderItem>();
            var sql = (from order in DbSet
                       join item in itemDbSet on order.Id equals item.OmsOrderId
                       group new { item, order } by item.OmsOrderId into gItems
                       select new OmsOrderAggr()
                       {
                           OrderId = gItems.First().order.Id,
                           Order = gItems.First().order,
                           Items = gItems.Select(s => s.item).OrderBy(o => o.ProductNo).ToList()
                       });
            return await sql.IgnoreQueryFilters().FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查询订单列表
        /// </summary>
        /// <param name="orderNos">订单编号</param>
        /// <returns></returns>
        public async Task<IEnumerable<OmsOrder>> GetListAsync(List<string> orderNos)
        {
            var nos = orderNos.ConvertAll(item => item.TryLong());
            return await DbSet.IgnoreQueryFilters().Where(w => nos.Contains(w.OrderNo)).ToListAsync();
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="form">参数表单</param>
        /// <returns>分页列表</returns>
        public async Task<PageList<OmsOrder>> GetPageAsync(int pageIndex, int pageSize, OmsGetPageOrderForm form)
        {
            var predicate = PredicateBuilder.Create<OmsOrder>(w => true);
            if (!form.OrderNo.IsNullOrEmpty())
            {
                var no = form.OrderNo.TryLong();
                predicate = predicate.And(w => w.OrderNo == no);
            }
            if (!form.UserName.IsNullOrEmpty())
                predicate = predicate.And(w => w.UserName == form.UserName);
            if (!form.Source.IsNullOrEmpty())
                predicate = predicate.And(w => w.Source == form.Source);
            if (!form.PlatformOrderNo.IsNullOrEmpty())
                predicate = predicate.And(w => w.PlatformOrderNo == form.PlatformOrderNo);
            if (!form.PayType.IsNullOrEmpty())
                predicate = predicate.And(w => w.PayType == form.PayType);
            if (!form.PayType.IsNullOrEmpty())
                predicate = predicate.And(w => w.ReceiverJson.Contains(form.ReceiverMobile));
            if (form.State != null)
                predicate = predicate.And(w => w.State == form.State);
            if (form.PayState != null)
                predicate = predicate.And(w => w.PayState == form.PayState);
            if (form.ShippingState != null)
                predicate = predicate.And(w => w.ShippingState == form.ShippingState);
            if (form.PayBeginTime != null)
                predicate = predicate.And(w => w.PayTime >= form.PayBeginTime);
            if (form.PayEndTime != null)
                predicate = predicate.And(w => w.PayTime <= form.PayEndTime);
            if (form.CreateBeginTime != null)
                predicate = predicate.And(w => w.CreateTime >= form.CreateBeginTime);
            if (form.CreateEndTime != null)
                predicate = predicate.And(w => w.CreateTime <= form.CreateEndTime);

            var total = await DbSet
                .CountAsync(predicate);

            var items = await DbSet
                .Where(predicate)
                .OrderByDescending(o => o.CreateTime)
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .ToListAsync();

            return new PageList<OmsOrder>(total, pageIndex, pageSize, items);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="userName">用户购买账号</param>
        /// <param name="state">订单状态</param>
        /// <param name="payState">支付状态（默认显示待支付）</param>
        /// <returns>分页列表</returns>
        public async Task<PageList<OmsOrder>> GetPagePersonalIQFAsync(
            int pageIndex,
            int pageSize,
            string userName,
            OmsOrderStateEnum? state,
            OmsOrderPayStateEnum? payState)
        {
            var predicate = PredicateBuilder.Create<OmsOrder>(w => w.UserName == userName);
            if (state != null)
                predicate = predicate.And(w => w.State == state);
            if (payState != null)
                predicate = predicate.And(w => w.PayState == payState);
            var total = await DbSet
                .IgnoreQueryFilters()
                .CountAsync(predicate);

            var items = await DbSet
                .IgnoreQueryFilters()
                .Where(predicate)
                .OrderByDescending(o => o.CreateTime)
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .ToListAsync();

            return new PageList<OmsOrder>(total, pageIndex, pageSize, items);
        }

        /// <summary>
        /// 查询指定付款状态订单
        /// </summary>
        /// <param name="state">付款状态</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<OmsOrder>> GetListIQFAsync(OmsOrderStateEnum state)
        {
            return await DbSet.IgnoreQueryFilters().Where(w => w.State == state).ToListAsync();
        }

        /// <summary>
        /// 查询指定日期的订单当日金额统计
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public async Task<OmsOrderTodayAmountVo> CountTodayAmountAsync(DateTime date)
        {
            var startDate = date.Date;
            var endDate = date.Date.AddDays(1);
            var sql = (from order in DbSet
                       where order.CreateTime >= startDate && order.CreateTime < endDate
                       select new
                       {
                           order.TotalPrice,
                           order.PaidAmount,
                           order.State
                       });

            var data = await sql.ToListAsync();
            return new OmsOrderTodayAmountVo()
            {
                TotalAmount = data.Sum(s => s.TotalPrice),
                TotalPaidAmount = data.Sum(s => s.PaidAmount),
                DealCount = data.Count,
                CancelCount = data.Where(w => w.State == OmsOrderStateEnum.Canceled).Count(),
            };
        }
    }
}
