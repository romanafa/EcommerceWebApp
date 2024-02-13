using AutoMapper;
using EcommerceWebApp_API.Data;
using EcommerceWebApp_API.Models.Category;
using EcommerceWebApp_API.Models.Product;

namespace EcommerceWebApp_API.Configurations
{
    public class MapperConfig : Profile
    {
        
        public MapperConfig()
        {
            // Mapping for Category entity
            CreateMap<CategoryReadOnlyDto, Category>().ReverseMap();
            CreateMap<CategoryCreateDto, Category>().ReverseMap();
            CreateMap<CategoryUpdateDto, Category>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();

            // Mapping for Product entity
            CreateMap<ProductReadOnlyDto, Product>().ReverseMap();
            CreateMap<ProductCreateDto, Product>().ReverseMap();
            CreateMap<ProductUpdateDto, Product>().ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();



        }

    }
}
