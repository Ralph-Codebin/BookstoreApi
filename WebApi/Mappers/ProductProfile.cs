
using AutoMapper;
using bookstore_api.Models;
using bookstore_api.Requests;
using Domain.Model.Entities;

namespace bookstore_api.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            //CreateMap<DNewProductRequest, NewSubscriptiontRequest>()
            //    .ForMember(dst => dst.title, opt => opt.MapFrom(src => src.title));

            CreateMap<DReturnObject, ReturnObject>()
                .ForMember(dst => dst.data, opt => opt.MapFrom(src => src.data));

            CreateMap<NewProductRequest, DNewProductRequest>()
            .ForMember(dst => dst.title, opt => opt.MapFrom(src => src.title));

            CreateMap<DUpdateProductDataRequest, UpdateProductDataRequest>()
            .ForMember(dst => dst.title, opt => opt.MapFrom(src => src.title));

            CreateMap<UpdateProductDataRequest, DUpdateProductDataRequest>()
            .ForMember(dst => dst.title, opt => opt.MapFrom(src => src.title));

        }
    }
}
