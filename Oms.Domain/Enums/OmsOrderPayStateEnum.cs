using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.Enums
{
    /// <summary>
    /// 订单付款状态
    /// </summary>
    public enum OmsOrderPayStateEnum
    {
        /// <summary>
        /// 待支付
        /// </summary>
        UnPay = 0,

        /// <summary>
        /// 已支付
        /// </summary>
        Paid = 1,

        /// <summary>
        /// 已退款
        /// </summary>
        Refunded = 2
    }
}
