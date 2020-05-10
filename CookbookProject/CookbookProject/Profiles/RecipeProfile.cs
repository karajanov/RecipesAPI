using AutoMapper;
using CookbookProject.DataTransferObjects;
using CookbookProject.Models;

namespace CookbookProject.Profiles
{
    public class RecipeProfile : Profile
    {
        public RecipeProfile()
        {
            CreateMap<Recipe, RecipeViewModel>()
                .ReverseMap()
                .ForMember(r => r.Category, opt => opt.Ignore())
                .ForMember(r => r.Cuisine, opt => opt.Ignore())
                .ForMember(r => r.User, opt => opt.Ignore())
                .ForMember(r => r.Ingredients, opt => opt.Ignore());
        }
    }
}
