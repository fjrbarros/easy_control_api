using AutoMapper;
using EasyControl.Api.Contract.User;
using EasyControl.Api.Domain.Models;

namespace EasyControl.Api.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserCreateRequestContract>().ReverseMap();
            CreateMap<User, UserCreateResponseContract>().ReverseMap();
            CreateMap<User, UserLoginResponseContract>().ReverseMap();
        }
    }
}
