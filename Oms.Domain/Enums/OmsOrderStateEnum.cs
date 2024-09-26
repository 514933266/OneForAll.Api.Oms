using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Description("待支付")]
        WaitingPay = 0,

        /// <summary>
        /// 已支付
        /// </summary>
        [Description("已支付")]
        Paid = 1,

        /// <summary>
        /// 已发货
        /// </summary>
        [Description("已发货")]
        Shipped = 2,

        /// <summary>
        /// 确认收货
        /// </summary>
        [Description("确认收货")]
        Received = 3,

        /// <summary>
        /// 退款中
        /// </summary>
        [Description("退款中")]
        Refunded = 4,

        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Completed = 5,

        /// <summary>
        /// 已关闭
        /// </summary>
        [Description("已关闭")]
        Closed = 6,

        /// <summary>
        /// 已取消
        /// </summary>
        [Description("已取消")]
        Canceled = 98,

        /// <summary>
        /// 异常订单
        /// </summary>
        [Description("异常")]
        Error = 99
    }
}
