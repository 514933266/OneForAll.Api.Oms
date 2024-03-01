using AutoMapper;
using Oms.Application.Dtos;
using Oms.Domain.AggregateRoots;
using Oms.Domain.Aggregates;
using Oms.Domain.Models;
using Oms.Domain.ValueObject;
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
            CreateMap<OmsOrder, OmsOrderDto>()
                 .ForMember(e => e.OrderNo, a => a.MapFrom(e => e.OrderNo.ToString()));
            CreateMap<OmsOrderAggr, OmsOrderDto>()
                 .ForMember(e => e.OrderNo, a => a.MapFrom(e => e.OrderNo.ToString()))
                .ForMember(e => e.Receiver, a => a.MapFrom(e => (e.ReceiverJson.FromJson<OmsOrderReceiverVo>())))
                .ForMember(e => e.OtherPrices, a => a.MapFrom(e => (e.OtherPriceJson.FromJson<IEnumerable<OmsOrderOtherPriceVo>>())))
                .ForMember(e => e.Items, a => a.MapFrom(e => e.OmsOrderItems));

            CreateMap<OmsOrderItem, OmsOrderItemDto>()
                .ForMember(e => e.ProductSnapshots, a => a.MapFrom(e => (e.ProductSnapshotJson.FromJson<IEnumerable<OmsOrderProductSnapshotVo>>())))
                .ForMember(e => e.OtherPrices, a => a.MapFrom(e => (e.OtherPriceJson.FromJson<IEnumerable<OmsOrderOtherPriceVo>>())));

            CreateMap<OmsOrder, OmsOrderAggr>();
            CreateMap<OmsOrder, OmsOrderCallBackAggr>();

            CreateMap<OmsOrderForm, OmsOrder>()
                .ForMember(e => e.SysTenantId, a => a.MapFrom(e => e.TenantId))
                .ForMember(e => e.ReceiverJson, a => a.MapFrom(e => (e.Receiver.ToJson())))
                .ForMember(e => e.OtherPriceJson, a => a.MapFrom(e => (e.OtherPrices.ToJson())));
            CreateMap<OmsOrderItemForm, OmsOrderItem>()
                .ForMember(e => e.OtherPriceJson, a => a.MapFrom(e => (e.OtherPrices.ToJson())))
                .ForMember(e => e.ProductSnapshotJson, a => a.MapFrom(e => (e.ProductSnapshots.ToJson())));
            CreateMap<OmsCustomOrderForm, OmsOrder>()
                .ForMember(e => e.SysTenantId, a => a.MapFrom(e => e.TenantId))
                .ForMember(e => e.ReceiverJson, a => a.MapFrom(e => (e.Receiver.ToJson())))
                .ForMember(e => e.OtherPriceJson, a => a.MapFrom(e => (e.OtherPrices.ToJson())));
        }
    }
}
