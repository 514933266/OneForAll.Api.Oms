using Oms.Domain.AggregateRoots;
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
    }
}
