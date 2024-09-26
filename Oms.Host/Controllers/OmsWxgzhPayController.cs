using Microsoft.AspNetCore.Mvc;
using Oms.Application.Interfaces;
using Oms.Domain.Models;
using OneForAll.Core;
using OneForAll.Core.Extension;
using System.Threading.Tasks;

namespace Oms.Host.Controllers
{
    /// <summary>
    /// 微信公众号支付
    /// </summary>
    [Route("open-api/[controller]")]
    public class OmsWxgzhPayController : BaseController
    {
        private readonly IOmsWxgzhPayService _service;
        public OmsWxgzhPayController(IOmsWxgzhPayService service)
        {
            _service = service;
        }

        /// <summary>
        /// 预创建订单，返回prepay_id调起微信支付
        /// </summary>
        /// <param name="form">订单</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseMessage> CreateOrderAsync([FromBody] OmsOrderForm form)
        {
            if (form.PayType.IsNullOrEmpty())
                form.PayType = "微信公众号支付";
            return await _service.CreateOrderAsync(form);
        }
    }
}

