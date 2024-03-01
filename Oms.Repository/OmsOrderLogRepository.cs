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
    /// 订单日志
    /// </summary>
    public class OmsOrderLogRepository : Repository<OmsOrderLog>, IOmsOrderLogRepository
    {
        public OmsOrderLogRepository(DbContext context)
            : base(context)
        {

        }
    }
}

