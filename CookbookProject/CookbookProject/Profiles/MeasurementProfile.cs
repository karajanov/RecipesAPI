using AutoMapper;
using CookbookProject.DataTransferObjects;
using CookbookProject.Models;

namespace CookbookProject.Profiles
{
    public class MeasurementProfile : Profile
    {
        public MeasurementProfile()
        {
            CreateMap<Measurement, MeasurementViewModel>()
                .ReverseMap()
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ForMember(m => m.Recipe, opt => opt.Ignore())
                .ForMember(m => m.Ingredient, opt => opt.Ignore());
        }
    }
}
