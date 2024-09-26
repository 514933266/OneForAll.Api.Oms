using Oms.Domain.AggregateRoots;
using Oms.Domain.Aggregates;
using Oms.Domain.Enums;
using Oms.Domain.Models;
using Oms.Domain.ValueObject;
using OneForAll.Core;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.Repositorys
{
    /// <summary>
    /// 订单
    /// </summary>
    public interface IOmsOrderRepository : IEFCoreRepository<OmsOrder>
    {
        /// <summary>
        /// 查询指定订单
        /// </summary>
        /// <returns>实体</returns>
        Task<OmsOrder> GetIQFAsync(Guid id);

        /// <summary>
        /// 查询指定订单
        /// </summary>
        /// <param name="orderNo">订单编号</param>
        /// <returns>实体</returns>
        Task<OmsOrder> GetIQFAsync(string orderNo);

        /// <summary>
        /// 查询指定订单
        /// </summary>
        /// <returns>实体</returns>
        Task<OmsOrderAggr> GetAggrIQFAsync(Guid id);

        /// <summary>
        /// 查询订单列表
        /// </summary>
        /// <param name="orderNos">订单编号</param>
        /// <returns></returns>
        Task<IEnumerable<OmsOrder>> GetListAsync(List<string> orderNos);

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="form">参数表单</param>
        /// <returns>分页列表</returns>
        Task<PageList<OmsOrder>> GetPageAsync(int pageIndex, int pageSize, OmsGetPageOrderForm form);

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="userName">用户购买账号</param>
        /// <param name="state">订单状态</param>
        /// <param name="payState">支付状态（默认显示待支付）</param>
        /// <returns>分页列表</returns>
        Task<PageList<OmsOrder>> GetPagePersonalIQFAsync(
            int pageIndex,
            int pageSize,
            string userName,
            OmsOrderStateEnum? state,
            OmsOrderPayStateEnum? payState);

        /// <summary>
        /// 查询指定付款状态订单
        /// </summary>
        /// <param name="state">订单状态</param>
        /// <returns>列表</returns>
        Task<IEnumerable<OmsOrder>> GetListIQFAsync(OmsOrderStateEnum state);

        /// <summary>
        /// 查询指定日期的订单当日金额统计
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        Task<OmsOrderTodayAmountVo> CountTodayAmountAsync(DateTime date);
    }
}
