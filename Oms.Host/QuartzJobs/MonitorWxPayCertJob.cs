using Oms.Public.Models;
using Quartz;
using Oms.Domain.Repositorys;
using System;
using System.Threading.Tasks;
using System.Linq;
using OneForAll.Core.Extension;
using Oms.HttpService.Interfaces;
using Oms.HttpService.Models;
using Oms.Public;
using System.IO;
using OneForAll.File;
using OneForAll.Core.Security;
using System.Text;
using System.Runtime.ConstrainedExecution;

namespace Oms.Host.QuartzJobs
{
    /// <summary>
    /// 微信支付证书刷新定时任务
    /// </summary>
    [DisallowConcurrentExecution]
    public class MonitorWxPayCertJob : IJob
    {
        private readonly AuthConfig _config;
        private readonly IOmsWxPaySettingRepository _wxPayRepository;
        private readonly IWxPayHttpService _payHttpService;
        private readonly IScheduleJobHttpService _jobHttpService;
        private readonly ISysGlobalExceptionLogHttpService _logHttpService;
        public MonitorWxPayCertJob(
            AuthConfig config,
            IOmsWxPaySettingRepository wxPayRepository,
            IWxPayHttpService payHttpService,
            IScheduleJobHttpService jobHttpService,
            ISysGlobalExceptionLogHttpService logHttpService)
        {
            _config = config;
            _wxPayRepository = wxPayRepository;
            _payHttpService = payHttpService;
            _jobHttpService = jobHttpService;
            _logHttpService = logHttpService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var num = 0;
                var pageIndex = 1;
                var pageSize = 100;
                var total = 1;

                while (pageIndex <= total)
                {
                    var data = await _wxPayRepository.GetPageIQFAsync(pageIndex, pageSize);
                    total = data.TotalPage;
                    data.Items.ForEach(async e =>
                    {
                        if (!e.IsAutoRefreshCert)
                        {
                            await _jobHttpService.LogAsync(_config.ClientCode, typeof(MonitorOrderCancelJob).Name, $"商户号：{e.Mchid}已设置不自动更新证书：跳过...");
                            return;
                        }
                        var keyPath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + e.CertificateKeyUrl);
                        var certPath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + e.CertificateUrl);
                        var privateKey = WxPayV3SDK.GetCertPrivateKey(keyPath);
                        if (FileHelper.CheckIsExists(certPath))
                        {
                            var certs = await _payHttpService.GetCertAsync(e.Mchid, e.CertSerialNo, privateKey);
                            var cert = certs.Data?.OrderByDescending(o => o.ExpireTime).FirstOrDefault();
                            if (certs.Data?.Count > 1)
                            {
                                // 当存在新旧两个证书时，说明支付证书即将过期，取最新的证书保存
                                var certText = AesGcmHelper.Decrypt(cert.EncryptCertificate.AssociatedData, cert.EncryptCertificate.Nonce, cert.EncryptCertificate.Ciphertext, e.APIv3Key);
                                FileHelper.Write(certPath, Encoding.UTF8.GetBytes(certText));
                                e.CreateTime = cert.EffectiveTime;
                                await _wxPayRepository.SaveChangesAsync();
                                await _jobHttpService.LogAsync(_config.ClientCode, typeof(MonitorOrderCancelJob).Name, $"商户号：{e.Mchid}更新证书成功，新证书编号：{cert.SerialNo}");
                            }
                            else if (cert != null)
                            {
                                await _jobHttpService.LogAsync(_config.ClientCode, typeof(MonitorOrderCancelJob).Name, $"商户号：{e.Mchid}证书未到期，过期时间：{cert.ExpireTime}");
                            }
                            else
                            {
                                await _jobHttpService.LogAsync(_config.ClientCode, typeof(MonitorOrderCancelJob).Name, $"商户号：{e.Mchid}证书跳过处理，未成功加载证书");
                            }
                        }
                    });
                    pageIndex++;
                }
                await _jobHttpService.LogAsync(_config.ClientCode, typeof(MonitorOrderCancelJob).Name, $"巡检微信支付证书刷新定时任务执行完成：共计取消{num}单");
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
