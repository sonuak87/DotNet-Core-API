using AutoMapper;
using VeggiFoodAPI.Models.DTOs;
using VeggiFoodAPI.Models.ViewModels;

namespace VeggiFoodAPI.RequestHelpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreateProductModel, Product>();
            CreateMap<UpdateProductModel, Product>();
        }
    }
}
