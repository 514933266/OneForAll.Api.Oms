using Oms.Application.Dtos;
using Oms.Domain.Enums;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Application.Interfaces
{
    /// <summary>
    /// 个人订单
    /// </summary>
    public interface IOmsPersonalOrderService
    {
        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <returns></returns>
        Task<OmsOrderDto> GetAsync(Guid orderId);

        /// <summary>
        /// 查询指定账号订单列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="userName">购买账号</param>
        /// <param name="state">订单状态</param>
        /// <param name="payState">支付状态（默认显示待支付）</param>
        /// <returns></returns>]
        Task<PageList<OmsOrderDto>> GetPgaeAsync(
            int pageIndex,
            int pageSize,
            string userName,
            OmsOrderStateEnum? state,
            OmsOrderPayStateEnum? payState);
    }
}
