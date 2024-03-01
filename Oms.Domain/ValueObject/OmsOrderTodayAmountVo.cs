using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.ValueObject
{
    /// <summary>
    /// 今日订单金额统计
    /// </summary>
    public class OmsOrderTodayAmountVo
    {
        /// <summary>
        /// 成交订单数量
        /// </summary>
        public int DealCount { get; set; }

        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal TotalPaidAmount { get; set; }

        /// <summary>
        /// 取消订单数量
        /// </summary>
        public int CancelCount { get; set; }
    }
}
