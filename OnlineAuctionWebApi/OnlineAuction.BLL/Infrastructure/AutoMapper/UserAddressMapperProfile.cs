using AutoMapper;
using OnlineAuction.BLL.DTO;
using OnlineAuction.DAL.Entities;

namespace OnlineAuction.BLL.Infrastructure.AutoMapper
{
    public class UserAddressMapperProfile : Profile
    {
        public UserAddressMapperProfile()
        {
            CreateMap<UserAddress, UserAddressDTO>().MaxDepth(1).ReverseMap();
        }
    }
}
