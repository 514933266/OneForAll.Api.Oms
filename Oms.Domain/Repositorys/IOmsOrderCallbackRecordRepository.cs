using Oms.Domain.AggregateRoots;
using Oms.Domain.Aggregates;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.Repositorys
{
    /// <summary>
    /// 订单回调记录
    /// </summary>
    public interface IOmsOrderCallbackRecordRepository : IEFCoreRepository<OmsOrderCallbackRecord>
    {
        /// <summary>
        /// 查询可以回传的数据列表
        /// </summary>
        /// <returns>实体</returns>
        Task<IEnumerable<OmsOrderCallbackRecordAggr>> GetListValidAsync();
    }
}
