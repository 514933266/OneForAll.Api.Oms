using Oms.Domain.AggregateRoots;
using Oms.Domain.Models;
using OneForAll.Core;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.Interfaces
{
    /// <summary>
    /// 订单回调记录
    /// </summary>
    public interface IOmsOrderCallbackRecordManager
    {
        /// <summary>
        /// 创建回调记录
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>调起微信支付的sign</returns>
        Task<BaseErrType> AddAsync(OmsOrderCallbackRecord form);

        /// <summary>
        /// 更新回调成功信息
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>调起微信支付的sign</returns>
        Task<BaseErrType> UpdateAsync(OmsOrderCallbackRecordUpdateForm form);
    }
}
