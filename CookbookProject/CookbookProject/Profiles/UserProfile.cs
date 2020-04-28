using AutoMapper;
using CookbookProject.DataTransferObjects;
using CookbookProject.Models;

namespace CookbookProject.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>()
                .ReverseMap();
        }
    }
}
