using Oms.HttpService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.HttpService.Models
{
    /// <summary>
    /// 系统通知
    /// </summary>
    public class UmsMessageRequest
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public UmsMessageTypeEnum Type { get; set; }

        /// <summary>
        /// 接收账号id
        /// </summary>
        public Guid ToAccountId { get; set; }
    }
}
