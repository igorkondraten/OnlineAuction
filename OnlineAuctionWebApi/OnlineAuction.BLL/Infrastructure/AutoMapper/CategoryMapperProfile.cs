using AutoMapper;
using OnlineAuction.BLL.DTO;
using OnlineAuction.DAL.Entities;

namespace OnlineAuction.BLL.Infrastructure.AutoMapper
{
    /// <summary>
    /// Automapper profile for category entity.
    /// </summary>
    public class CategoryMapperProfile : Profile
    {
        public CategoryMapperProfile()
        {
            CreateMap<Category, CategoryDTO>().MaxDepth(1).ReverseMap();
        }
    }
}
