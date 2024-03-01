using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.Enums
{
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OmsOrderStateEnum
    {
        /// <summary>
        /// 待支付
        /// </summary>
        WaitingPay = 0,

        /// <summary>
        /// 已支付
        /// </summary>
        Paid = 1,

        /// <summary>
        /// 已发货
        /// </summary>
        Shipped = 2,

        /// <summary>
        /// 确认收货
        /// </summary>
        Received = 3,

        /// <summary>
        /// 退款中
        /// </summary>
        Refunded = 4,

        /// <summary>
        /// 已完成
        /// </summary>
        Completed = 5,

        /// <summary>
        /// 关闭
        /// </summary>
        Closed = 6,

        /// <summary>
        /// 已取消
        /// </summary>
        Canceled = 98,

        /// <summary>
        /// 异常订单
        /// </summary>
        Error = 99
    }
}
