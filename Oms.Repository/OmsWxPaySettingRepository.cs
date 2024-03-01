using Microsoft.EntityFrameworkCore;
using Oms.Domain.AggregateRoots;
using Oms.Domain.Repositorys;
using OneForAll.Core.ORM;
using OneForAll.Core;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneForAll.Core.Extension;

namespace Oms.Repository
{
    /// <summary>
    /// 微信支付设置
    /// </summary>
    public class OmsWxPaySettingRepository : Repository<OmsWxPaySetting>, IOmsWxPaySettingRepository
    {
        public OmsWxPaySettingRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>分页列表</returns>
        public async Task<PageList<OmsWxPaySetting>> GetPageAsync(int pageIndex, int pageSize, string key)
        {
            var predicate = PredicateBuilder.Create<OmsWxPaySetting>(w => true);
            if (!key.IsNullOrEmpty())
            {
                predicate = PredicateBuilder.Create<OmsWxPaySetting>(w => w.AppId.Contains(key) || w.Mchid.Contains(key));
            }

            var total = await DbSet.CountAsync(predicate);
            var data = await DbSet
                .AsNoTracking()
                .Where(predicate)
                .OrderByDescending(e => e.CreateTime)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageList<OmsWxPaySetting>(total, pageSize, pageIndex, data);
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="ids">实体id</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<OmsWxPaySetting>> GetListAsync(IEnumerable<Guid> ids)
        {
            return await DbSet.Where(w => ids.Contains(w.Id)).ToListAsync();
        }

        /// <summary>
        /// 查询指定商户设置
        /// </summary>
        /// <returns>实体</returns>
        public async Task<OmsWxPaySetting> GetIQFAsync(Guid id)
        {
            return await DbSet.IgnoreQueryFilters().FirstOrDefaultAsync(w=>w.Id == id);
        }

        /// <summary>
        /// 查询指定商户设置
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <param name="mchid">商户号</param>
        /// <returns>实体</returns>
        public async Task<OmsWxPaySetting> GetIQFAsync(Guid tenantId, string mchid)
        {
            var predicate = PredicateBuilder.Create<OmsWxPaySetting>(w => w.SysTenantId == tenantId);
            if (!mchid.IsNullOrEmpty())
                predicate = predicate.And(w => w.Mchid == mchid);
            return await DbSet.IgnoreQueryFilters().Where(predicate).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 统计数量
        /// </summary>
        /// <returns>分页列表</returns>
        public async Task<int> CountAsync()
        {
            return await DbSet.CountAsync();
        }
    }
}
