using Oms.Public.Models;
using Quartz;
using Oms.Application.Interfaces;
using Oms.Domain.Interfaces;
using Oms.Domain.Repositorys;
using Oms.Host.Models;
using System;
using System.Threading.Tasks;
using System.Linq;
using OneForAll.Core.Extension;
using Oms.Domain.AggregateRoots;
using OneForAll.Core;
using System.Net.Http.Formatting;
using System.Net.Http;
using Oms.Domain.Models;
using Oms.HttpService.Interfaces;
using NPOI.SS.Formula.Functions;
using Oms.HttpService.Models;

namespace Oms.Host.QuartzJobs
{
    /// <summary>
    /// 订单回调定时任务
    /// </summary>
    [DisallowConcurrentExecution]
    public class MonitorOrderCallbackJob : IJob
    {
        private readonly AuthConfig _config;
        private readonly IOmsOrderCallbackRecordManager _callbackManager;
        private readonly IOmsOrderCallbackRecordRepository _repository;
        private readonly IScheduleJobHttpService _jobHttpService;
        private readonly ISysGlobalExceptionLogHttpService _logHttpService;
        public MonitorOrderCallbackJob(
            AuthConfig config,
            IOmsOrderCallbackRecordManager callbackManager,
            IOmsOrderCallbackRecordRepository repository,
            IScheduleJobHttpService jobHttpService,
            ISysGlobalExceptionLogHttpService logHttpService)
        {
            _config = config;
            _callbackManager = callbackManager;
            _repository = repository;
            _jobHttpService = jobHttpService;
            _logHttpService = logHttpService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                // 超过3600分钟的将不再回调
                var callbacks = await _repository.GetListValidAsync();
                var orderIds = callbacks.Select(s => s.OmsOrderId).ToList();
                if (orderIds.Count > 0)
                {
                    callbacks.ForEach(async e =>
                    {
                        if (e.LastUpdateTime.AddMinutes(e.TimeStep) <= DateTime.Now && !e.CallBackUrl.IsNullOrEmpty())
                            await SynOrderAsync(e.CallBackUrl, e.Order);

                    });
                }
                await _jobHttpService.LogAsync(_config.ClientCode, typeof(MonitorOrderCallbackJob).Name, $"巡检支付订单回传执行完成：共计回传{orderIds.Count}单");
            }
            catch (Exception ex)
            {
                await SendGlobalExceptionAsync(ex);
            }
        }

        // 回传订单信息
        private async Task<BaseErrType> SynOrderAsync(string url, OmsOrder order)
        {
            /* 此版本忽略签名校验以及确认返回流程，后续补上 */
            var client = new HttpClient();
            var response = await client.PostAsync(url, order, new JsonMediaTypeFormatter());
            var result = await response.Content.ReadAsAsync<BaseMessage>();
            var record = new OmsOrderCallbackRecordUpdateForm()
            {
                OrderId = order.Id,
                Error = result.Message,
                IsSuccess = result.Status
            };
            var errType = await _callbackManager.UpdateAsync(record);
            await _jobHttpService.LogAsync(_config.ClientCode, typeof(MonitorOrderCallbackJob).Name, $"订单id：{order.Id},流水号：{order.OrderNo},回调{(result.Status ? "成功" : "失败,原因：" + result.Message)}");
            return errType;
        }

        // 发送全局异常
        private async Task SendGlobalExceptionAsync(Exception ex)
        {
            await _logHttpService.AddAsync(new SysGlobalExceptionLogRequest
            {
                MoudleName = _config.ClientName,
                MoudleCode = _config.ClientCode,
                Name = ex.Message,
                Content = ex.InnerException == null ? ex.StackTrace : ex.InnerException.StackTrace
            });
        }
    }
}
