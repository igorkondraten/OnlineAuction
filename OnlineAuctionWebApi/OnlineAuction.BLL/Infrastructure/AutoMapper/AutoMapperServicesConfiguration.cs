using AutoMapper;

namespace OnlineAuction.BLL.Infrastructure.AutoMapper
{
    public static class AutoMapperServicesConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new UserMapperProfile());
                cfg.AddProfile(new LotMapperProfile());
                cfg.AddProfile(new CategoryMapperProfile());
                cfg.AddProfile(new BidMapperProfile());
                cfg.AddProfile(new UserAddressMapperProfile());
            });
        }
    }
}
