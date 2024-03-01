using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.Enums
{
    /// <summary>
    /// 发货状态
    /// </summary>
    public enum OmsOrderShippingStateEnum
    {
        /// <summary>
        /// 待发货
        /// </summary>
        Pending = 0,

        /// <summary>
        /// 已发货
        /// </summary>
        Shipped = 1,

        /// <summary>
        /// 已收货
        /// </summary>
        Received = 2,

        /// <summary>
        /// 已退货
        /// </summary>
        Returned = 99
    }
}
