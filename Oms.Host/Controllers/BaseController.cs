using System;
using System.Linq;
using Oms.Domain.Models;
using Oms.Host.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OneForAll.Core.Extension;
using Oms.Public.Models;

namespace Oms.Host.Controllers
{
    public class BaseController : Controller
    {
        protected LoginUser LoginUser
        {
            get
            {
                var claims = HttpContext.User.Claims;
                if (claims.Any())
                {
                    return new LoginUser()
                    {
                        Id = claims.FirstOrDefault(e => e.Type == UserClaimType.USER_ID).Value.TryGuid(),
                        SysTenantId = claims.FirstOrDefault(e => e.Type == UserClaimType.TENANT_ID).Value.TryGuid(),
                        Name = claims.FirstOrDefault(e => e.Type == UserClaimType.USER_NICKNAME)?.Value,
                        UserName = claims.FirstOrDefault(e => e.Type == UserClaimType.USERNAME)?.Value,
                        IsDefault = claims.FirstOrDefault(e => e.Type == UserClaimType.IS_DEFAULT).Value.TryBoolean(),
                        WxAppId = claims.FirstOrDefault(e => e.Type == UserClaimType.WX_APPID)?.Value,
                        WxOpenId = claims.FirstOrDefault(e => e.Type == UserClaimType.WX_OPENID)?.Value,
                        WxUnionId = claims.FirstOrDefault(e => e.Type == UserClaimType.WX_UNIONID)?.Value
                    };
                }
                return new LoginUser();
            }
        }
    }
}