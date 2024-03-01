using Oms.Domain.AggregateRoots;
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
    /// 订单日志
    /// </summary>
    public interface IOmsOrderLogManager
    {
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="form">订单</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddAsync(OmsOrderLogForm form);

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="forms">订单</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddAsync(IEnumerable<OmsOrderLogForm> forms);
    }
}
