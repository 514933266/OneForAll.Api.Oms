using Microsoft.EntityFrameworkCore;
using Oms.Domain.AggregateRoots;
using Oms.Domain.Aggregates;
using Oms.Domain.Enums;
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
    /// 订单回调记录
    /// </summary>
    public class OmsOrderCallbackRecordRepository : Repository<OmsOrderCallbackRecord>, IOmsOrderCallbackRecordRepository
    {
        public OmsOrderCallbackRecordRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 查询可以回传的数据列表
        /// </summary>
        /// <returns>实体</returns>
        public async Task<IEnumerable<OmsOrderCallbackRecordAggr>> GetListValidAsync()
        {
            var orderDbSet = Context.Set<OmsOrder>().Where(w => w.PayState == OmsOrderPayStateEnum.Paid);
            var sql = from callback in DbSet
                      join order in orderDbSet on callback.OmsOrderId equals order.Id
                      where !callback.IsSuccess && callback.TimeStep <= 3600
                      select new OmsOrderCallbackRecordAggr()
                      {
                          OmsOrderId = callback.OmsOrderId,
                          CallBackUrl = callback.CallBackUrl,
                          IsSuccess = callback.IsSuccess,
                          Error = callback.Error,
                          CreateTime = callback.CreateTime,
                          LastUpdateTime = callback.LastUpdateTime,
                          SuccessTime = callback.SuccessTime,
                          TimeStep = callback.TimeStep,
                          Order = order
                      };

            return await sql.ToListAsync();
        }
    }
}
