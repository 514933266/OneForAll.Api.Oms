using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using OneForAll.Core.DDD;
using Oms.Public.Models;
using Oms.Domain.AggregateRoots;

namespace Oms.Domain
{
    /// <summary>
    /// 基类
    /// </summary>
    public class OmsBaseManager : BaseManager
    {
        protected readonly IMapper _mapper;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public OmsBaseManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        protected Guid UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext
                .User
                .Claims
                .FirstOrDefault(e => e.Type == UserClaimType.USER_ID);

                if (userId != null)
                {
                    return new Guid(userId.Value);
                }
                return Guid.Empty;
            }
        }

        protected string UserName
        {
            get
            {
                var username = _httpContextAccessor.HttpContext
                .User
                .Claims
                .FirstOrDefault(e => e.Type == UserClaimType.USERNAME);

                if (username != null)
                {
                    return username.Value;
                }
                return null;
            }
        }

        protected Guid SysTenantId
        {
            get
            {
                var tenantId = _httpContextAccessor.HttpContext
                .User
                .Claims
                .FirstOrDefault(e => e.Type == UserClaimType.TENANT_ID);

                if (tenantId != null)
                {
                    return new Guid(tenantId.Value);
                }
                return Guid.Empty;
            }
        }

        protected LoginUser LoginUser
        {
            get
            {
                var name = _httpContextAccessor.HttpContext
                .User
                .Claims
                .FirstOrDefault(e => e.Type == UserClaimType.USER_NICKNAME);

                var role = _httpContextAccessor.HttpContext
                .User
                .Claims
                .FirstOrDefault(e => e.Type == UserClaimType.ROLE);

                return new LoginUser()
                {
                    Id = UserId,
                    Name = name.Value,
                    SysTenantId = SysTenantId,
                    IsDefault = role.Value.Equals(UserRoleType.RULER)
                };
            }
        }
    }
}
