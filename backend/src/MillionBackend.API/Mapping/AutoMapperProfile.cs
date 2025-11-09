using AutoMapper;
using MillionBackend.API.DTOs;
using MillionBackend.Core.Models;

namespace MillionBackend.API.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Property mappings
        CreateMap<Property, PropertyDto>()
            .ForMember(dest => dest.IdProperty, opt => opt.MapFrom(src => src.IdProperty))
            .ForMember(dest => dest.MainImage, opt => opt.MapFrom(src =>
                src.Images != null && src.Images.Any() ? src.Images.First().File : null));

        CreateMap<Property, PropertyListDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdProperty))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src =>
                src.Images != null && src.Images.Any() ? src.Images.First().File : string.Empty));

        CreateMap<Property, PropertyDetailDto>()
            .ForMember(dest => dest.IdProperty, opt => opt.MapFrom(src => src.IdProperty))
            .ForMember(dest => dest.MainImage, opt => opt.MapFrom(src =>
                src.Images != null && src.Images.Any() ? src.Images.First().File : null));

        CreateMap<CreatePropertyDto, Property>()
            .ForMember(dest => dest.IdProperty, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Enabled, opt => opt.MapFrom(_ => true));

        CreateMap<DTOs.UpdatePropertyDto, Property>()
            .ForMember(dest => dest.IdProperty, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Map API DTO to Core DTO
        CreateMap<DTOs.UpdatePropertyDto, Core.Models.UpdatePropertyDto>();

        // Map Core DTO to Property
        CreateMap<Core.Models.UpdatePropertyDto, Property>()
            .ForMember(dest => dest.IdProperty, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IdOwner, opt => opt.Ignore())
            .ForMember(dest => dest.Owner, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.Traces, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Owner mappings
        CreateMap<Owner, OwnerDto>()
            .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.IdOwner));

        CreateMap<CreateOwnerDto, Owner>()
            .ForMember(dest => dest.IdOwner, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // PropertyImage mappings
        CreateMap<PropertyImage, PropertyImageDto>()
            .ForMember(dest => dest.IdPropertyImage, opt => opt.MapFrom(src => src.IdPropertyImage));

        CreateMap<CreatePropertyImageDto, PropertyImage>()
            .ForMember(dest => dest.IdPropertyImage, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        // PropertyTrace mappings
        CreateMap<PropertyTrace, PropertyTraceDto>()
            .ForMember(dest => dest.IdPropertyTrace, opt => opt.MapFrom(src => src.IdPropertyTrace));

        CreateMap<CreatePropertyTraceDto, PropertyTrace>()
            .ForMember(dest => dest.IdPropertyTrace, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
    }
}