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
    public class BidsService : IBidsService, IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;

        public BidsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

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

        public async Task DeleteBidAsync(int bidId)
        {
            var savedBid = await _unitOfWork.Bids.GetAsync(bidId);
            if (savedBid == null)
                throw new NotFoundException("Bid not found.");
            _unitOfWork.Bids.Delete(bidId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<BidDTO>> GetBidsByLotAsync(int lotId)
        {
            var bids = await _unitOfWork.Bids.GetAllByLotAsync(lotId);
            return (bids.Select(x => Mapper.Map<Bid, BidDTO>(x)));
        }

        public async Task<IEnumerable<BidDTO>> GetBidsByUserAsync(int userId)
        {
            var bids = await _unitOfWork.Bids.GetAllByUserAsync(userId);
            return bids.Select(x => Mapper.Map<Bid, BidDTO>(x));
        }

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
