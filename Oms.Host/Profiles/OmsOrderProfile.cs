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
                .ForMember(e => e.Id, a => a.MapFrom(e => e.Order.Id))
                .ForMember(e => e.OrderNo, a => a.MapFrom(e => e.Order.OrderNo))
                .ForMember(e => e.PlatformOrderNo, a => a.MapFrom(e => e.Order.PlatformOrderNo))
                .ForMember(e => e.Source, a => a.MapFrom(e => e.Order.Source))
                .ForMember(e => e.UserId, a => a.MapFrom(e => e.Order.UserId))
                .ForMember(e => e.UserName, a => a.MapFrom(e => e.Order.UserName))
                .ForMember(e => e.PlatformPayerId, a => a.MapFrom(e => e.Order.PlatformPayerId))
                .ForMember(e => e.ProductName, a => a.MapFrom(e => e.Order.ProductName))
                .ForMember(e => e.State, a => a.MapFrom(e => e.Order.State))
                .ForMember(e => e.PayState, a => a.MapFrom(e => e.Order.PayState))
                .ForMember(e => e.ShippingState, a => a.MapFrom(e => e.Order.ShippingState))
                .ForMember(e => e.TotalPrice, a => a.MapFrom(e => e.Order.TotalPrice))
                .ForMember(e => e.PaidAmount, a => a.MapFrom(e => e.Order.PaidAmount))
                .ForMember(e => e.Currency, a => a.MapFrom(e => e.Order.Currency))
                .ForMember(e => e.Discount, a => a.MapFrom(e => e.Order.Discount))
                .ForMember(e => e.OffsetAmount, a => a.MapFrom(e => e.Order.OffsetAmount))
                .ForMember(e => e.PayType, a => a.MapFrom(e => e.Order.PayType))
                .ForMember(e => e.PayTime, a => a.MapFrom(e => e.Order.PayTime))
                .ForMember(e => e.ShippingMethod, a => a.MapFrom(e => e.Order.ShippingMethod))
                .ForMember(e => e.ShippingPrice, a => a.MapFrom(e => e.Order.ShippingPrice))
                .ForMember(e => e.IssuedInvoice, a => a.MapFrom(e => e.Order.IssuedInvoice))
                .ForMember(e => e.CreateTime, a => a.MapFrom(e => e.Order.CreateTime))
                .ForMember(e => e.UpdateTime, a => a.MapFrom(e => e.Order.UpdateTime))
                .ForMember(e => e.MayFailureTime, a => a.MapFrom(e => e.Order.MayFailureTime))
                .ForMember(e => e.Remark, a => a.MapFrom(e => e.Order.Remark))
                .ForMember(e => e.Receiver, a => a.MapFrom(e => (e.Order.ReceiverJson.FromJson<OmsOrderReceiverVo>())))
                .ForMember(e => e.OtherPrices, a => a.MapFrom(e => (e.Order.OtherPriceJson.FromJson<IEnumerable<OmsOrderOtherPriceVo>>())));

            CreateMap<OmsOrderItem, OmsOrderItemDto>()
                .ForMember(e => e.ProductSnapshots, a => a.MapFrom(e => (e.ProductSnapshotJson.FromJson<IEnumerable<OmsOrderProductSnapshotVo>>())))
                .ForMember(e => e.OtherPrices, a => a.MapFrom(e => (e.OtherPriceJson.FromJson<IEnumerable<OmsOrderOtherPriceVo>>())));

            CreateMap<OmsOrder, OmsOrderAggr>()
                .ForMember(e => e.OrderId, a => a.MapFrom(e => e.Id))
                .ForMember(e => e.Order, a => a.MapFrom(e => e));

            CreateMap<OmsOrder, OmsOrderCallBackAggr>()
                .ForMember(e => e.TenantId, a => a.MapFrom(e => e.SysTenantId));

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
