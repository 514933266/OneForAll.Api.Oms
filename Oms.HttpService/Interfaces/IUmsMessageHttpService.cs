using OneForAll.Core;
using Oms.HttpService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.HttpService.Interfaces
{
    /// <summary>
    /// 消息通知
    /// </summary>
    public interface IUmsMessageHttpService
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns></returns>
        Task<BaseMessage> SendAsync(UmsMessageForm form);
    }
}
