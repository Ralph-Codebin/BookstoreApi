using Application.Requests.LoginRequests;
using Application.Responses.LoginResponses;
using AutoMapper;

namespace bookstore_api.Mappers
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            CreateMap<PostLoginRequest, CreateBearerToken>();
            CreateMap<BearerToken, PostLoginResponse>();
        }
    }
}
