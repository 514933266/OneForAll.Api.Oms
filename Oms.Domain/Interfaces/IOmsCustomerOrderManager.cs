using Oms.Domain.Aggregates;
using Oms.Domain.Enums;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.Interfaces
{
    /// <summary>
    /// 个人订单
    /// </summary>
    public interface IOmsCustomerOrderManager
    {
        /// <summary>
        /// 查询订单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OmsOrderAggr> GetAsync(Guid id);

        /// <summary>
        /// 查询订单列表
        /// </summary>
        /// <param name="orderNos">订单编号</param>
        /// <returns></returns>
        Task<IEnumerable<OmsOrderAggr>> GetListAsync(List<string> orderNos);

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="userName">购买账号</param>
        /// <param name="state">订单状态</param>
        /// <param name="payState">支付状态（默认显示待支付）</param>
        ///  <returns>分页</returns>
        Task<PageList<OmsOrderAggr>> GetPgaeAsync(
            int pageIndex,
            int pageSize,
            string userName,
            OmsOrderStateEnum? state,
            OmsOrderPayStateEnum? payState);
    }
}
