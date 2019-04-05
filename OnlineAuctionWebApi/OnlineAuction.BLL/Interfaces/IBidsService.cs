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
        Task<(IEnumerable<BidDTO> Bids, int TotalCount)> GetBidsByLotAsync(int lotId, int limit, int offset);
        Task<(IEnumerable<BidDTO> Bids, int TotalCount)> GetBidsByUserAsync(int userId, int limit, int offset);
        Task<BidDTO> GetBidAsync(int bidId);
    }
}
