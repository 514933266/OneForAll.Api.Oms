using AutoMapper;
using Oms.Application.Dtos;
using Oms.Application.Interfaces;
using Oms.Domain.Interfaces;
using Oms.Domain.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Application
{
    /// <summary>
    /// 订单
    /// </summary>
    public class OmsOrderService : IOmsOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOmsOrderManager _areaManageer;

        public OmsOrderService(
            IMapper mapper,
            IOmsOrderManager areaManageer)
        {
            _mapper = mapper;
            _areaManageer = areaManageer;
        }
    }
}

