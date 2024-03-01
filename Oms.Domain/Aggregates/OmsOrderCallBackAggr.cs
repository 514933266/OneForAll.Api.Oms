using Oms.Domain.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.Aggregates
{
    /// <summary>
    /// 订单回调
    /// </summary>
    public class OmsOrderCallBackAggr : OmsOrder
    {
        /// <summary>
        /// 订单明细
        /// </summary>
        public List<OmsOrderItem> Items { get; set; } = new List<OmsOrderItem>();
    }
}
