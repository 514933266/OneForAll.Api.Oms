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
using Oms.Domain.Models;
using Oms.HttpService.Interfaces;
using Oms.Domain.Enums;
using System.Collections.Generic;
using NPOI.SS.Formula.Functions;
using Oms.HttpService.Models;

namespace Oms.Host.QuartzJobs
{
    /// <summary>
    /// 取消订单定时任务
    /// </summary>
    [DisallowConcurrentExecution]
    public class MonitorOrderCancelJob : IJob
    {
        private readonly AuthConfig _config;
        private readonly IOmsOrderLogManager _logManager;
        private readonly IOmsOrderCallbackRecordManager _cbManager;

        private readonly IOmsOrderRepository _repository;
        private readonly IScheduleJobHttpService _jobHttpService;
        private readonly ISysGlobalExceptionLogHttpService _logHttpService;
        public MonitorOrderCancelJob(
            AuthConfig config,
            IOmsOrderLogManager logManager,
            IOmsOrderCallbackRecordManager cbManager,
            IOmsOrderRepository repository,
            IScheduleJobHttpService jobHttpService,
            ISysGlobalExceptionLogHttpService logHttpService)
        {
            _config = config;
            _logManager = logManager;
            _cbManager = cbManager;
            _repository = repository;
            _jobHttpService = jobHttpService;
            _logHttpService = logHttpService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var num = 0;
                var orders = await _repository.GetListIQFAsync(OmsOrderStateEnum.WaitingPay);
                var ids = new List<Guid>();
                if (orders.Any())
                {
                    var logs = new List<OmsOrderLogForm>();
                    orders.ForEach(e =>
                    {
                        if (e.MayFailureTime <= DateTime.Now)
                        {
                            ids.Add(e.Id);
                            e.State = OmsOrderStateEnum.Canceled;
                            e.UpdateTime = DateTime.Now;
                            logs.Add(new OmsOrderLogForm()
                            {
                                OrderId = e.Id,
                                PayState = e.PayState,
                                State = e.State,
                                ShippingState = e.ShippingState,
                                Remark = "订单超时取消"
                            });
                        }
                    });
                    num = await _repository.SaveChangesAsync();
                    if (logs.Any())
                        await _logManager.AddAsync(logs);
                    if (ids.Any())
                        await SynCallbackRecordAsync(ids);
                }
                await _jobHttpService.LogAsync(_config.ClientCode, typeof(MonitorOrderCancelJob).Name, $"巡检过期订单执行完成：共计取消{num}单");
            }
            catch (Exception ex)
            {
                await SendGlobalExceptionAsync(ex);
            }
        }

        // 更新回传记录表，避免重复回传
        private async Task SynCallbackRecordAsync(IEnumerable<Guid> orderIds)
        {
            if (orderIds.Any())
            {
                orderIds.ForEach(async orderId =>
                {
                    await _cbManager.SynOrderAsync(orderId);
                });
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
