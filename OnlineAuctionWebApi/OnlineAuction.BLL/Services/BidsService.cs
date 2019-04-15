using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using OnlineAuction.BLL.DTO;
using OnlineAuction.BLL.Enums;
using OnlineAuction.BLL.Exceptions;
using OnlineAuction.BLL.Interfaces;
using OnlineAuction.DAL.Entities;
using OnlineAuction.DAL.Interfaces;

namespace OnlineAuction.BLL.Services
{
    /// <summary>
    /// Contains methods for managing bids.
    /// </summary>
    public class BidsService : IBidsService, IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;

        public BidsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Async method for creating bid.
        /// </summary>
        /// <param name="bid">The bid DTO.</param>
        /// <returns>The Task, containing created bid DTO.</returns>
        /// <exception cref="ArgumentNullException">Thrown if bid is null.</exception>
        /// <exception cref="ValidationException">Thrown when validation is failed.</exception>
        public async Task<BidDTO> CreateBidAsync(BidDTO bid)
        {
            if (bid == null)
                throw new ArgumentNullException(nameof(bid), "Bid is null.");
            var user = (await _unitOfWork.UserProfiles.FindAsync(x => x.Name == bid.PlacedUserName)).Items
                .FirstOrDefault();
            if (user == null)
                throw new ValidationException("User not found.");
            if (bid.Price <= 0)
                throw new ValidationException("Incorrect price.");
            var lot = Mapper.Map<Lot, LotDTO>(await _unitOfWork.Lots.GetAsync(bid.LotId));
            if (lot.Status != AuctionStatus.Active)
                throw new ValidationException("Auction is not active.");
            if (lot.CurrentPrice >= bid.Price)
                throw new ValidationException("Current price is higher than bid price. Current price: " +
                                              lot.CurrentPrice.ToString("0.##"));
            var newBid = Mapper.Map<BidDTO, Bid>(bid);
            newBid.Date = DateTime.UtcNow;
            newBid.PlacedUserId = user.UserProfileId;
            _unitOfWork.Bids.Create(newBid);
            await _unitOfWork.SaveAsync();
            return Mapper.Map<Bid, BidDTO>(await _unitOfWork.Bids.GetAsync(newBid.BidId));
        }

        /// <summary>
        /// Async method for deleting bid.
        /// </summary>
        /// <param name="bidId">The bid ID.</param>
        /// <returns>The Task.</returns>
        /// <exception cref="NotFoundException">Thrown when bid not found in DB.</exception>
        public async Task DeleteBidAsync(int bidId)
        {
            var savedBid = await _unitOfWork.Bids.GetAsync(bidId);
            if (savedBid == null)
                throw new NotFoundException("Bid not found.");
            _unitOfWork.Bids.Delete(bidId);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Async method for getting bids by lot ID.
        /// </summary>
        /// <param name="lotId">The lot ID.</param>
        /// <returns>The Task, containing collection of bids DTOs.</returns>
        /// <exception cref="NotFoundException">Thrown when lot not found in DB.</exception>
        public async Task<IEnumerable<BidDTO>> GetBidsByLotAsync(int lotId)
        {
            if (await _unitOfWork.Lots.GetAsync(lotId) == null)
                throw new NotFoundException("Lot not found");
            var bids = await _unitOfWork.Bids.GetAllByLotAsync(lotId);
            return (bids.Select(x => Mapper.Map<Bid, BidDTO>(x)));
        }

        /// <summary>
        /// Async method for getting bids by user profile ID.
        /// </summary>
        /// <param name="userProfileId">The user profile ID.</param>
        /// <returns>The Task, containing collection of bids DTOs.</returns>
        /// <exception cref="NotFoundException">Thrown when user not found in DB.</exception>
        public async Task<IEnumerable<BidDTO>> GetBidsByUserAsync(int userProfileId)
        {
            if (await _unitOfWork.UserProfiles.GetAsync(userProfileId) == null)
                throw new NotFoundException("User not found");
            var bids = await _unitOfWork.Bids.GetAllByUserAsync(userProfileId);
            return bids.Select(x => Mapper.Map<Bid, BidDTO>(x));
        }

        /// <summary>
        /// Async method for getting bid by ID.
        /// </summary>
        /// <param name="bidId">The bid ID.</param>
        /// <returns>The Task, containing bid DTO.</returns>
        public async Task<BidDTO> GetBidAsync(int bidId)
        {
            return Mapper.Map<Bid, BidDTO>(await _unitOfWork.Bids.GetAsync(bidId));
        }

        #region IDisposable Support
        private bool _isDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _unitOfWork.Dispose();
                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
