using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Oms.Domain.Enums;
using Oms.Domain.ValueObject;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.AggregateRoots
{
    /// <summary>
    /// 订单记录
    /// </summary>
    public class OmsOrderLog
    {
        /// <summary>
        /// 订单id
        /// </summary>
        [Key]
        [Required]
        public Guid OmsOrderId { get; set; }

        /// <summary>
        /// 操作记录Json
        /// </summary>
        [Required]
        public string DetailJson { get; set; }

        /// <summary>
        /// 获取创建订单日志
        /// </summary>
        /// <param name="state">订单状态</param>
        /// <param name="payState">订单支付状态</param>
        /// <param name="remark">备注</param>
        /// <returns>日志</returns>
        public void AddDetail(OmsOrderStateEnum state, OmsOrderPayStateEnum payState, string remark = "")
        {
            if (DetailJson.IsNullOrEmpty())
                DetailJson = new List<OmsOrderLogDetailVo>().ToJson();
            var logs = DetailJson.FromJson<List<OmsOrderLogDetailVo>>();
            var log = new OmsOrderLogDetailVo()
            {
                CreatorName = "系统操作",
                State = state,
                PayState = payState
            };
            if (logs.Any(w => w.State == state && w.PayState == payState))
                return;

            if (remark.IsNullOrEmpty())
            {
                switch (state)
                {
                    case OmsOrderStateEnum.WaitingPay: log.Remark = "创建订单"; break;
                    case OmsOrderStateEnum.Paid: log.Remark = "支付订单"; break;
                    case OmsOrderStateEnum.Canceled: log.Remark = "取消订单"; break;
                }
            }
            else
            {
                log.Remark = remark;
            }
            logs.Add(log);
            DetailJson = logs.ToJson();
        }
    }
}
