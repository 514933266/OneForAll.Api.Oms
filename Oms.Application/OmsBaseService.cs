﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Oms.Public.Models;
using OneForAll.Core.Extension;
using OneForAll.Core.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Application
{
    /// <summary>
    /// 基础服务类
    /// </summary>
    public class OmsBaseService
    {
        protected readonly IMapper _mapper;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public OmsBaseService(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        protected LoginUser LoginUser
        {
            get
            {
                var claims = _httpContextAccessor.HttpContext?.User.Claims;
                if (claims.Any())
                {
                    return new LoginUser()
                    {
                        Name = claims.FirstOrDefault(e => e.Type == UserClaimType.USER_NICKNAME)?.Value ?? "",
                        UserName = claims.FirstOrDefault(e => e.Type == UserClaimType.USERNAME)?.Value ?? "",
                        WxAppId = claims.FirstOrDefault(e => e.Type == UserClaimType.WX_APPID)?.Value ?? "",
                        WxOpenId = claims.FirstOrDefault(e => e.Type == UserClaimType.WX_OPENID)?.Value ?? "",
                        WxUnionId = claims.FirstOrDefault(e => e.Type == UserClaimType.WX_UNIONID)?.Value ?? "",
                        Id = claims.FirstOrDefault(e => e.Type == UserClaimType.USER_ID).Value.TryGuid(),
                        SysTenantId = claims.FirstOrDefault(e => e.Type == UserClaimType.TENANT_ID).Value.TryGuid(),
                        IsDefault = claims.FirstOrDefault(e => e.Type == UserClaimType.IS_DEFAULT).Value.TryBoolean()
                    };
                }
                return new LoginUser();
            }
        }
    }
}
