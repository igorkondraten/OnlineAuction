using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineAuction.BLL.DTO;

namespace OnlineAuction.BLL.Interfaces
{
    /// <summary>
    /// Interface for bids service.
    /// Contains methods for managing bids.
    /// </summary>
    public interface IBidsService : IDisposable
    {
        /// <summary>
        /// Async method for creating bid.
        /// </summary>
        /// <param name="bid">The bid DTO.</param>
        /// <returns>The Task, containing created bid DTO.</returns>
        Task<BidDTO> CreateBidAsync(BidDTO bid);

        /// <summary>
        /// Async method for deleting bid.
        /// </summary>
        /// <param name="bidId">The bid ID.</param>
        /// <returns>The Task.</returns>
        Task DeleteBidAsync(int bidId);

        /// <summary>
        /// Async method for getting bids by lot ID.
        /// </summary>
        /// <param name="lotId">The lot ID.</param>
        /// <returns>The Task, containing collection of bids DTOs.</returns>
        Task<IEnumerable<BidDTO>> GetBidsByLotAsync(int lotId);

        /// <summary>
        /// Async method for getting bids by user profile ID.
        /// </summary>
        /// <param name="userProfileId">The user profile ID.</param>
        /// <returns>The Task, containing collection of bids DTOs.</returns>
        Task<IEnumerable<BidDTO>> GetBidsByUserAsync(int userProfileId);

        /// <summary>
        /// Async method for getting bid by ID.
        /// </summary>
        /// <param name="bidId">The bid ID.</param>
        /// <returns>The Task, containing bid DTO.</returns>
        Task<BidDTO> GetBidAsync(int bidId);
    }
}
