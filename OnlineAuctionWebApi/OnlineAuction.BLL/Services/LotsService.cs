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
    public class LotsService : ILotsService, IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;

        public LotsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<LotDTO> CreateLotAsync(LotDTO lot)
        {
            if (lot == null)
                throw new ArgumentNullException(nameof(lot), "Lot is null.");
            if (lot.BeginDate < DateTime.UtcNow || lot.EndDate < DateTime.UtcNow || lot.BeginDate > lot.EndDate)
                throw new ValidationException("Incorrect date.");
            if ((lot.EndDate - lot.BeginDate).TotalHours < 1)
                throw new ValidationException("Auction duration can not be less than 1 hour.");
            if (lot.InitialPrice <= 0)
                throw new ValidationException("Price must be greater than zero.");
            var user = (await _unitOfWork.UserProfiles.FindAsync(x => x.Name == lot.UserName)).Items.FirstOrDefault();
            if (user == null)
                throw new ValidationException("User not found.");
            var category = await _unitOfWork.Categories.GetAsync(lot.Category.CategoryId);
            if (category == null)
                throw new ValidationException("Category not found.");
            var newLot = Mapper.Map<LotDTO, Lot>(lot);
            newLot.UserId = user.UserProfileId;
            _unitOfWork.Lots.Create(newLot);
            await _unitOfWork.SaveAsync();
            return Mapper.Map<Lot, LotDTO>(await _unitOfWork.Lots.GetAsync(newLot.LotId));
        }

        public async Task EditLotAsync(LotDTO lot)
        {
            if (lot == null)
                throw new ArgumentNullException(nameof(lot), "Lot is null.");
            var oldLot = await _unitOfWork.Lots.GetAsync(lot.LotId);
            if (oldLot == null)
                throw new ArgumentException("Lot not found.", nameof(lot));
            if ((lot.EndDate - lot.BeginDate).TotalHours < 1)
                throw new ValidationException("Auction duration can not be less than 1 hour.");
            var category = await _unitOfWork.Categories.GetAsync(lot.Category.CategoryId);
            if (category == null)
                throw new ValidationException("Category not found.");
            var oldLotDto = Mapper.Map<Lot, LotDTO>(oldLot);
            if (oldLotDto.Status != AuctionStatus.New)
            {
                if (oldLotDto.BeginDate != lot.BeginDate)
                    throw new ValidationException("Can't edit auction begin date after it has started.");
                if (oldLotDto.EndDate != lot.EndDate && lot.EndDate < DateTime.UtcNow)
                    throw new ValidationException("Auction end date must be higher than current date.");
            }
            lot.InitialPrice = oldLot.InitialPrice;
            _unitOfWork.Lots.Update(Mapper.Map<LotDTO, Lot>(lot, oldLot));
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteLotAsync(int lotId)
        {
            if (await _unitOfWork.Lots.GetAsync(lotId) == null)
                throw new NotFoundException("Lot not found.");
            _unitOfWork.Lots.Delete(lotId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<(IEnumerable<LotDTO> Lots, int TotalCount)> GetAllLotsAsync(int limit, int offset)
        {
            var (lots, totalCount) = await _unitOfWork.Lots.GetAllAsync(limit, offset);
            return (lots.Select(x => Mapper.Map<Lot, LotDTO>(x)).ToList(), totalCount);
        }

        public async Task<(IEnumerable<LotDTO> Lots, int TotalCount)> FindLotsAsync(string keywords, int limit, int offset)
        {
            var (lots, totalCount) = string.IsNullOrEmpty(keywords)
                ? await _unitOfWork.Lots.GetAllAsync(limit, offset)
                : await _unitOfWork.Lots.FindAsync(x => x.Name.Contains(keywords), limit, offset);
            return (lots.Select(x => Mapper.Map<Lot, LotDTO>(x)), totalCount);
        }

        public async Task<(IEnumerable<LotDTO> Lots, int TotalCount)> GetLotsByUserAsync(int userId, int limit, int offset)
        {
            var (lots, totalCount) = await _unitOfWork.Lots.FindAsync(x => x.UserId == userId, limit, offset);
            return (lots.Select(x => Mapper.Map<Lot, LotDTO>(x)), totalCount);
        }

        public async Task<LotDTO> GetLotAsync(int lotId)
        {
            return Mapper.Map<Lot, LotDTO>(await _unitOfWork.Lots.GetAsync(lotId)); ;
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
