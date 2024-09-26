using Oms.Application.Dtos;
using Oms.Domain.Models;
using Oms.Domain.ValueObject;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Application.Interfaces
{
    /// <summary>
    /// 订单
    /// </summary>
    public interface IOmsOrderService
    {
        /// <summary>
        /// 查询指定日期的订单当日金额统计
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        Task<OmsOrderTodayAmountVo> GetTodayAmountAsync(DateTime date);

        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <returns></returns>
        Task<OmsOrderDto> GetAsync(Guid orderId);

        /// <summary>
        /// 查询订单列表
        /// </summary>
        /// <param name="orderNos">订单编号</param>
        /// <returns></returns>
        Task<IEnumerable<OmsOrderDto>> GetListAsync(List<string> orderNos);

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="form">参数表单</param>
        /// <returns>分页列表</returns>
        Task<PageList<OmsOrderDto>> GetPageAsync(int pageIndex, int pageSize, OmsGetPageOrderForm form);

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>创建结果</returns>
        Task<BaseErrType> AddAsync(OmsCustomOrderForm form);
    }
}
