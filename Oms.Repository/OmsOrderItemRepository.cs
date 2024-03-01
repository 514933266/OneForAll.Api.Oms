using Microsoft.EntityFrameworkCore;
using Oms.Domain.AggregateRoots;
using Oms.Domain.Repositorys;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Repository
{
    /// <summary>
    /// 订单明细
    /// </summary>
    public class OmsOrderItemRepository : Repository<OmsOrderItem>, IOmsOrderItemRepository
    {
        public OmsOrderItemRepository(DbContext context)
            : base(context)
        {

        }
    }
}
