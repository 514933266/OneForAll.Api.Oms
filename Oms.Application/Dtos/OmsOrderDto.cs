using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Oms.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oms.Domain.ValueObject;

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
        public string OrderNo { get; set; }

        /// <summary>
        /// 第三方订单编号（微信/支付宝/其他）
        /// </summary>
        public string PlatformOrderNo { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 购买用户id
        /// </summary>

        public string UserId { get; set; }

        /// <summary>
        /// 购买账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 支付平台的用户id
        /// </summary>
        public string PlatformPayerId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

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
        /// CNY：人民币，境内商户号仅支持人民币
        /// </summary>
        public string Currency { get; set; }

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
        public string PayType { get; set; }

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
        public OmsOrderReceiverVo Receiver { get; set; }

        /// <summary>
        /// 其他费用明细
        /// </summary>
        public List<OmsOrderOtherPriceVo> OtherPrices { get; set; } = new List<OmsOrderOtherPriceVo>();

        /// <summary>
        /// 已开发票
        /// </summary>
        public bool IssuedInvoice { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 预计失效时间
        /// </summary>
        public DateTime? MayFailureTime { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 订单明细
        /// </summary>
        public List<OmsOrderItemDto> Items { get; set; } = new List<OmsOrderItemDto>();
    }
}
