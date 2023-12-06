using AutoMapper;
using Oms.Application.Dtos;
using Oms.Domain.AggregateRoots;
using Oms.Domain.Models;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Oms.Host.Profiles
{
    public class OmsOrderProfile : Profile
    {
        public OmsOrderProfile()
        {
            CreateMap<OmsOrder, OmsOrderDto>();
            CreateMap<OmsOrderCreateForm, OmsOrder>()
                .ForMember(e => e.ReceiverJson, a => a.MapFrom(e => (e.Receiver.ToJson())))
                .ForMember(e => e.OtherPriceJson, a => a.MapFrom(e => (e.OtherPrice.ToJson())));
        }
    }
}
