﻿using Oms.Domain.Enums;
using Oms.Public.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Application.Dtos
{
    /// <summary>
    /// 微信小程序下单
    /// </summary>
    public class OmsWxmpCreateOrderDto
    {
        /// <summary>
        /// 系统订单id
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public long OrderNo { get; set; }

        /// <summary>
        /// 付款状态
        /// </summary>
        public OmsOrderPayStateEnum PayState { get; set; }

        /// <summary>
        /// 微信调起支付的数据
        /// </summary>
        public WxmpPayData PayData { get; set; }
    }
}
