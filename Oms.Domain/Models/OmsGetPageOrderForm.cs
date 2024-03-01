using Oms.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.Models
{
    /// <summary>
    /// 获取订单分页列表
    /// </summary>
    public class OmsGetPageOrderForm
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 购买用户账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 收件人电话
        /// </summary>
        public string ReceiverMobile { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 第三方平台单号
        /// </summary>
        public string PlatformOrderNo { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayType { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OmsOrderStateEnum? State { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        public OmsOrderPayStateEnum? PayState { get; set; }

        /// <summary>
        /// 物流状态
        /// </summary>
        public OmsOrderShippingStateEnum? ShippingState { get; set; }

        /// <summary>
        /// 创建开始时间
        /// </summary>
        public DateTime? CreateBeginTime { get; set; }

        /// <summary>
        /// 创建结束时间
        /// </summary>
        public DateTime? CreateEndTime { get; set; }

        /// <summary>
        /// 支付开始时间
        /// </summary>
        public DateTime? PayBeginTime { get; set; }

        /// <summary>
        /// 支付结束时间
        /// </summary>
        public DateTime? PayEndTime { get; set; }
    }
}
