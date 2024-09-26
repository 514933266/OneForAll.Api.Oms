using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oms.HttpService.Models
{
    /// <summary>
    /// 数据资源服务配置
    /// </summary>
    public class HttpServiceConfig
    {
        /// <summary>
        /// 权限验证接口
        /// </summary>
        public string SysPermissionCheck { get; set; } = "SysPermissionCheck";

        /// <summary>
        /// Api日志
        /// </summary>
        public string SysApiLog { get; set; } = "SysApiLog";

        /// <summary>
        /// 异常日志
        /// </summary>
        public string SysExceptionLog { get; set; } = "SysExceptionLog";

        /// <summary>
        /// 全局异常日志
        /// </summary>
        public string SysGlobalExceptionLog { get; set; } = "SysGlobalExceptionLog";

        /// <summary>
        /// 操作日志
        /// </summary>
        public string SysOperationLog { get; set; } = "SysOperationLog";

        /// <summary>
        /// 消息通知
        /// </summary>
        public string UmsMessage { get; set; } = "UmsMessage";

        /// <summary>
        /// 微信JSAPI下单
        /// </summary>
        public string WxJSAPIOrder { get; set; } = "WxJSAPIOrder";

        /// <summary>
        /// 微信小程序下单
        /// </summary>
        public string WxmpOrder { get; set; } = "WxmpOrder";

        /// <summary>
        /// 微信商户证书
        /// </summary>
        public string WxPayCert { get; set; } = "WxPayCert";

        /// <summary>
        /// 定时任务调度中心
        /// </summary>
        public string ScheduleJob { get; set; } = "ScheduleJob";

    }
}
