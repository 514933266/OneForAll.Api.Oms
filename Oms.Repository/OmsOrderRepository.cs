using Microsoft.EntityFrameworkCore;
using OneForAll.Core.ORM;
using OneForAll.Core;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oms.Domain.AggregateRoots;
using Oms.Domain.Repositorys;

namespace Oms.Repository
{
    /// <summary>
    /// 订单
    /// </summary>
    public class OmsOrderRepository : Repository<OmsOrder>, IOmsOrderRepository
    {
        public OmsOrderRepository(DbContext context)
            : base(context)
        {

        }
    }
}
