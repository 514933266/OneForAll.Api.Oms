using Oms.Domain.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.Aggregates
{
    /// <summary>
    /// 有效的订单回传记录
    /// </summary>
    public class OmsOrderCallbackRecordAggr : OmsOrderCallbackRecord
    {
        public OmsOrder Order { get; set; }
    }
}
