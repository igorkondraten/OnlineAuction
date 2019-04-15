using System;
using System.Linq;
using AutoMapper;
using OnlineAuction.BLL.DTO;
using OnlineAuction.BLL.Enums;
using OnlineAuction.DAL.Entities;

namespace OnlineAuction.BLL.Infrastructure.AutoMapper
{
    /// <summary>
    /// Automapper profile for lot entity.
    /// </summary>
    public class LotMapperProfile : Profile
    {
        public LotMapperProfile()
        {
            CreateMap<Lot, LotDTO>()
                .ForMember(x => x.Status, o => o.MapFrom(s =>
                    s.BeginDate >= DateTime.UtcNow ? AuctionStatus.New :
                    s.EndDate <= DateTime.UtcNow ? AuctionStatus.Finished : AuctionStatus.Active
                ))
                .ForMember(x => x.BestBid, o => o.MapFrom(s => s.Bids.LastOrDefault()))
                .ForMember(x => x.UserName, o => o.MapFrom(s => s.User.Name))
                .ForMember(x => x.CurrentPrice, o => o.MapFrom(s => (s.Bids.Count != 0) ? s.Bids.Last().Price : s.InitialPrice))
                .MaxDepth(1);
            CreateMap<LotDTO, Lot>()
                .ForMember(x => x.CategoryId, o => o.MapFrom(s => s.Category.CategoryId))
                .ForMember(x => x.Category, o => o.Ignore())
                .ForMember(x => x.Bids, o => o.Ignore())
                .MaxDepth(1);
        }
    }
}
