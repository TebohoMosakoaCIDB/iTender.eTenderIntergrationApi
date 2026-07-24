using AutoMapper;
using iTender.Application.DTOs;
using iTender.Domain.Models;

namespace iTender.Application.AutoMapperProfiles
{
    public class CreateTenderMapper : Profile
    {
        public CreateTenderMapper()
        {
            CreateMap<CreateTenderModel, TenderModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TypeOfContractName, opt => opt.Ignore())
                .ForMember(dest => dest.EmployerName, opt => opt.Ignore())
                .ForMember(dest => dest.LocalMunicipalityName, opt => opt.Ignore())
                .ForMember(dest => dest.MetroDistrictName, opt => opt.Ignore())
                .ForMember(dest => dest.ProvinceName, opt => opt.Ignore())
                .ForMember(dest => dest.SubCategoryName, opt => opt.Ignore())
                .ForMember(dest => dest.AlternateSubCategoryName, opt => opt.Ignore())
                .ForMember(dest => dest.TenderValueRangeName, opt => opt.Ignore())
                .ForMember(dest => dest.ClassOfConstructionWorksName, opt => opt.Ignore())
                .ForMember(dest => dest.AlternateClassOfConstructionWorksName, opt => opt.Ignore())
                .ForMember(dest => dest.StatusCodeId,
                    opt => opt.MapFrom(src => src.Status.HasValue
                        ? (int)src.Status.Value
                        : (int?)null))
                .ForMember(dest => dest.ClarificationMeetingRequired, opt => opt.Ignore());

            CreateMap<TenderModel, CreateTenderModel>()
                .ForMember(dest => dest.ClarificationMeetingRequired,
                    opt => opt.MapFrom(src =>
                        !string.IsNullOrWhiteSpace(src.ClarificationMeetingRequired)
                        && src.ClarificationMeetingRequired.Equals("Yes", StringComparison.OrdinalIgnoreCase)));
        }
    }
}
