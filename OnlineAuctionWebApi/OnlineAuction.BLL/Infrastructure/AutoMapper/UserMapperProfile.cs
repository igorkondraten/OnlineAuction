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
                .MaxDepth(1);
            CreateMap<ApplicationUser, UserDTO>()
                .ForMember(x => x.Name, o => o.MapFrom(s => s.UserName))
                .ForMember(x => x.UserProfileId, o => o.MapFrom(s => s.UserProfile.UserProfileId))
                .ForMember(x => x.Address, o => o.MapFrom(s => s.UserProfile.Address))
                .ForMember(x => x.Bids, o => o.MapFrom(s => s.UserProfile.Bids))
                .ForMember(x => x.Lots, o => o.MapFrom(s => s.UserProfile.Lots))
                .ForMember(x => x.RegistrationDate, o => o.MapFrom(s => s.UserProfile.RegistrationDate))
                .MaxDepth(1);
        }
    }
}
