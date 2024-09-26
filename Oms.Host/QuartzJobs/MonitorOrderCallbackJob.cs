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
using StackExchange.Redis;
using Oms.Domain.Aggregates;

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
                        {
                            var result = await _callbackManager.SynOrderAsync(e.CallBackUrl, e.OmsOrderId);
                            var order = result.Data as OmsOrderAggr;
                            await _jobHttpService.LogAsync(_config.ClientCode, typeof(MonitorOrderCallbackJob).Name, $"订单id：{order?.Order?.Id},流水号：{order?.Order?.OrderNo},回调{(result.Status ? "成功" : "失败,原因：" + result.Message)}");
                        }

                    });
                }
                await _jobHttpService.LogAsync(_config.ClientCode, typeof(MonitorOrderCallbackJob).Name, $"巡检支付订单回传执行完成：共计回传{orderIds.Count}单");
            }
            catch (Exception ex)
            {
                await SendGlobalExceptionAsync(ex);
            }
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
