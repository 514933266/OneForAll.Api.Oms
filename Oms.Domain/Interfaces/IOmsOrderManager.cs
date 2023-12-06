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
        /// 创建订单
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        Task<BaseMessage> CreateAsync(OmsOrderCreateForm form);
    }
}
