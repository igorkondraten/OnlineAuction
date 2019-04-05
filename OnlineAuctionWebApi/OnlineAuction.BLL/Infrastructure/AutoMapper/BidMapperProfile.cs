using AutoMapper;
using OnlineAuction.BLL.DTO;
using OnlineAuction.DAL.Entities;

namespace OnlineAuction.BLL.Infrastructure.AutoMapper
{
    public class BidMapperProfile : Profile
    {
        public BidMapperProfile()
        {
            CreateMap<Bid, BidDTO>()
                .ForMember(x => x.PlacedUserName, o => o.MapFrom(s => s.PlacedUser.Name))
                .MaxDepth(1);
            CreateMap<BidDTO, Bid>().MaxDepth(1);
        }
    }
}
