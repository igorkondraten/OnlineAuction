using System.Linq;
using AutoMapper;
using OnlineAuction.BLL.DTO;
using OnlineAuction.DAL.Entities;

namespace OnlineAuction.BLL.Infrastructure.AutoMapper
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<UserProfile, UserDTO>()
                .ForMember(x => x.Email, o => o.MapFrom(s => s.ApplicationUser.Email))
                .ForMember(x => x.Role, o => o.MapFrom(s => s.ApplicationUser.Roles.Select(x => x.RoleId).FirstOrDefault()))
                .MaxDepth(1);
            CreateMap<UserDTO, UserProfile>()
                .MaxDepth(1);
            CreateMap<UserDTO, ApplicationUser>()
                .ForMember(x => x.UserName, o => o.MapFrom(s => s.Name))
                .ForMember(x => x.Email, o => o.MapFrom(s => s.Email))
                .MaxDepth(1)
                .ReverseMap();
        }
    }
}
