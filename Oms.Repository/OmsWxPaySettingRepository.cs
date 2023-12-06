using Microsoft.EntityFrameworkCore;
using Oms.Domain.AggregateRoots;
using Oms.Domain.Repositorys;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
