
using AutoMapper;
using bookstore_api.Models;
using bookstore_api.Requests;
using Domain.Model.Entities;

namespace bookstore_api.Mappers
{
    public class SubscriptionProfile : Profile
    {
        public SubscriptionProfile()
        {
            CreateMap<NewSubscriptiontRequest, DNewSubscriptiontRequest>()
                .ForMember(dst => dst.prodid, opt => opt.MapFrom(src => src.prodid));

            CreateMap<UpdateSubscriptiontRequest, DUpdateSubscriptiontRequest>()
               .ForMember(dst => dst.id, opt => opt.MapFrom(src => src.id));
        }
    }
}
