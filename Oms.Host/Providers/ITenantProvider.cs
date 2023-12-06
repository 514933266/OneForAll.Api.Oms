using System;
using System.Collections.Generic;
using System.Text;

namespace Oms.Host
{
    public interface ITenantProvider
    {
        Guid GetTenantId();
    }
}
