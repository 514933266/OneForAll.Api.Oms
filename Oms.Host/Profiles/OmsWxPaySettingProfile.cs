using AutoMapper;
using Oms.Application.Dtos;
using Oms.Domain.AggregateRoots;
using Oms.Domain.Models;
using OneForAll.Core.Extension;

namespace Oms.Host.Profiles
{
    public class OmsWxPaySettingProfile : Profile
    {
        public OmsWxPaySettingProfile()
        {
            CreateMap<OmsWxPaySetting, OmsWxPaySettingDto>()
                .ForMember(e => e.IsUploadCert, a => a.MapFrom(e => !e.CertificateUrl.IsNullOrEmpty()));
            CreateMap<OmsWxPaySettingForm, OmsWxPaySetting>();
        }
    }
}
