﻿using Microsoft.AspNetCore.Http;
using Oms.HttpService.Interfaces;
using Oms.HttpService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Oms.HttpService
{
    /// <summary>
    /// 异常日志
    /// </summary>
    public class SysExceptionLogHttpService : BaseHttpService, ISysExceptionLogHttpService
    {
        private readonly HttpServiceConfig _config;

        public SysExceptionLogHttpService(
            HttpServiceConfig config,
            IHttpContextAccessor httpContext,
            IHttpClientFactory httpClientFactory) : base(httpContext, httpClientFactory)
        {
            _config = config;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns></returns>
        public async Task AddAsync(SysExceptionLogRequest form)
        {
            form.CreatorId = LoginUser.Id;
            form.CreatorName = LoginUser.Name;
            form.TenantId = LoginUser.SysTenantId;
            form.CreateTime = DateTime.Now;

            var client = GetHttpClient(_config.SysExceptionLog);
            if (client != null && client.BaseAddress != null)
            {
                var response = await client.PostAsync(client.BaseAddress, form, new JsonMediaTypeFormatter());
                var str = await response.Content.ReadAsStringAsync();
            }
        }
    }
}
