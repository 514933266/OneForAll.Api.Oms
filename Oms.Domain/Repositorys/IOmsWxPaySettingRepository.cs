using Oms.Domain.AggregateRoots;
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
    }
}
