using Oms.Domain.AggregateRoots;
using OneForAll.Core;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.Repositorys
{
    /// <summary>
    /// 微信支付设置
    /// </summary>
    public interface IOmsWxPaySettingRepository : IEFCoreRepository<OmsWxPaySetting>
    {
        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>分页列表</returns>
        Task<PageList<OmsWxPaySetting>> GetPageAsync(int pageIndex, int pageSize, string key);

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="ids">实体id</param>
        /// <returns>列表</returns>
        Task<IEnumerable<OmsWxPaySetting>> GetListAsync(IEnumerable<Guid> ids);

        /// <summary>
        /// 查询制定商户设置
        /// </summary>
        /// <returns>实体</returns>
        Task<OmsWxPaySetting> GetIQFAsync(Guid id);

        /// <summary>
        /// 查询指定商户设置
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <param name="mchid">商户号</param>
        /// <returns>实体</returns>
        Task<OmsWxPaySetting> GetIQFAsync(Guid tenantId, string mchid);

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <returns>分页列表</returns>
        Task<PageList<OmsWxPaySetting>> GetPageIQFAsync(int pageIndex, int pageSize);

        /// <summary>
        /// 统计数量
        /// </summary>
        /// <returns>分页列表</returns>
        Task<int> CountAsync();
    }
}
