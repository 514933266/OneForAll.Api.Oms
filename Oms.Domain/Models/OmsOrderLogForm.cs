using Oms.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.Models
{
    /// <summary>
    /// 订单日志
    /// </summary>
    public class OmsOrderLogForm
    {
        /// <summary>
        /// 订单id
        /// </summary>
        [Required]
        public Guid OrderId { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        [Required]
        public OmsOrderStateEnum State { get; set; }

        /// <summary>
        /// 付款状态
        /// </summary>
        [Required]
        public OmsOrderPayStateEnum PayState { get; set; }

        /// <summary>
        /// 发货状态
        /// </summary>
        [Required]
        public OmsOrderShippingStateEnum ShippingState { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
