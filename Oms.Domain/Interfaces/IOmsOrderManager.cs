using Oms.Domain.AggregateRoots;
using Oms.Domain.Aggregates;
using Oms.Domain.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.Interfaces
{
    /// <summary>
    /// 订单管理
    /// </summary>
    public interface IOmsOrderManager
    {
        /// <summary>
        /// 查询订单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OmsOrderAggr> GetAsync(Guid id);

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="form">参数表单</param>
        /// <returns>分页列表</returns>
        Task<PageList<OmsOrderAggr>> GetPageAsync(int pageIndex, int pageSize, OmsGetPageOrderForm form);

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>创建结果</returns>
        Task<BaseErrType> AddAsync(OmsCustomOrderForm form);

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="form">实体</param>
        /// <param name="setting">预计失效时间</param>
        /// <returns>调起微信支付的sign</returns>
        Task<OmsOrder> CreateAsync(OmsOrderForm form, OmsWxPaySetting setting);

        /// <summary>
        /// 更新第三方平台订单号
        /// </summary>
        /// <param name="id">订单id</param>
        /// <param name="orderNo">第三方平台订单号</param>
        /// <returns></returns>
        Task<BaseErrType> UpdatePlatformOrderNo(Guid id, string orderNo);
    }
}
