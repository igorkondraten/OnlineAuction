using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineAuction.BLL.DTO;

namespace OnlineAuction.BLL.Interfaces
{
    public interface IBidsService : IDisposable
    {
        Task<BidDTO> CreateBidAsync(BidDTO bid);
        Task DeleteBidAsync(int bidId);
        Task<IEnumerable<BidDTO>> GetBidsByLotAsync(int lotId);
        Task<IEnumerable<BidDTO>> GetBidsByUserAsync(int userId);
        Task<BidDTO> GetBidAsync(int bidId);
    }
}
