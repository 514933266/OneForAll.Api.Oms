﻿using Oms.Host.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using Oms.Public.Models;
using OneForAll.Core.OAuth;

namespace Oms.Host
{
    public class TenantProvider : ITenantProvider
    {
        private IHttpContextAccessor _context;

        public TenantProvider(IHttpContextAccessor context)
        {
            _context = context;
        }

        public Guid GetTenantId()
        {
            var tenantId = _context.HttpContext?.User.Claims.FirstOrDefault(e => e.Type == UserClaimType.TENANT_ID);
            if (tenantId != null)
            {
                return new Guid(tenantId.Value);
            }
            else
            {
                return Guid.Empty;
            }
        }
    }
}
