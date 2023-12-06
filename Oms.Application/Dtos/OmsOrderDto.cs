using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Oms.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Application.Dtos
{
    /// <summary>
    /// 订单
    /// </summary>
    public class OmsOrderDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 购买账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OmsOrderStateEnum State { get; set; }

        /// <summary>
        /// 付款状态
        /// </summary>
        public OmsOrderPayStateEnum PayState { get; set; }

        /// <summary>
        /// 发货状态
        /// </summary>
        public OmsOrderShippingStateEnum ShippingState { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 实付
        /// </summary>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// 抵扣金额
        /// </summary>
        public decimal OffsetAmount { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayType { get; set; } = "";

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 配送方式
        /// </summary>
        public string ShippingMethod { get; set; }

        /// <summary>
        /// 快递费
        /// </summary>
        public decimal ShippingPrice { get; set; }

        /// <summary>
        /// 收货人信息Json
        /// </summary>
        public string ReceiverJson { get; set; }

        /// <summary>
        /// 其他费用明细
        /// </summary>
        public string OtherPriceJson { get; set; }

        /// <summary>
        /// 已开发票
        /// </summary>
        public bool IssuedInvoice { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        public string Remark { get; set; } = "";
    }
}
