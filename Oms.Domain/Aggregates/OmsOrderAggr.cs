using Oms.Domain.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.Aggregates
{
    /// <summary>
    /// 订单
    /// </summary>
    public class OmsOrderAggr : OmsOrder
    {
        /// <summary>
        /// 订单明细
        /// </summary>
        public List<OmsOrderItem> OmsOrderItems { get; set; } = new List<OmsOrderItem>();
    }
}
