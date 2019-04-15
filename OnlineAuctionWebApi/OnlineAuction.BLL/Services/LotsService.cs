using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AutoMapper;
using OnlineAuction.BLL.DTO;
using OnlineAuction.BLL.Enums;
using OnlineAuction.BLL.Exceptions;
using OnlineAuction.BLL.Infrastructure;
using OnlineAuction.BLL.Interfaces;
using OnlineAuction.DAL.Entities;
using OnlineAuction.DAL.Interfaces;

namespace OnlineAuction.BLL.Services
{
    /// <summary>
    /// Interface for lots service.
    /// Contains methods for managing lots.
    /// </summary>
    public class LotsService : ILotsService, IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;

        public LotsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Async method for creating lot.
        /// </summary>
        /// <param name="lot">The lot DTO.</param>
        /// <returns>The Task, containing created lot DTO.</returns>
        /// <exception cref="ArgumentNullException">Thrown if lot is null.</exception>
        /// <exception cref="ValidationException">Thrown if lot validation failed.</exception>
        /// <exception cref="ArgumentException">Thrown if image can't be processed and saved.</exception>
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
            if (lot.Image != null)
            {
                try
                {
                    newLot.ImageUrl = ImageHandler.WriteImageToFile(lot.Image);
                }
                catch (ExternalException e)
                {
                    throw new ArgumentException("Processing image error.", nameof(lot.Image), e);
                }
            }
            newLot.UserId = user.UserProfileId;
            _unitOfWork.Lots.Create(newLot);
            await _unitOfWork.SaveAsync();
            return Mapper.Map<Lot, LotDTO>(await _unitOfWork.Lots.GetAsync(newLot.LotId));
        }

        /// <summary>
        /// Async method for updating lot.
        /// </summary>
        /// <param name="lot">The lot DTO.</param>
        /// <returns>The Task.</returns>
        /// <exception cref="ArgumentNullException">Thrown if lot is null.</exception>
        /// <exception cref="ArgumentException">Thrown if image can't be processed and saved.</exception>
        /// <exception cref="NotFoundException">Thrown if lot not found in DB.</exception>
        /// <exception cref="ValidationException">Thrown if lot validation failed.</exception>
        public async Task EditLotAsync(LotDTO lot)
        {
            if (lot == null)
                throw new ArgumentNullException(nameof(lot), "Lot is null.");
            var oldLot = await _unitOfWork.Lots.GetAsync(lot.LotId);
            if (oldLot == null)
                throw new NotFoundException("Lot not found.");
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
            if (lot.Image != null)
            {
                try
                {
                    lot.ImageUrl = ImageHandler.WriteImageToFile(lot.Image);
                }
                catch (ExternalException e)
                {
                    throw new ArgumentException("Processing image error.", nameof(lot.Image), e);
                }
            }
            lot.InitialPrice = oldLot.InitialPrice;
            _unitOfWork.Lots.Update(Mapper.Map<LotDTO, Lot>(lot, oldLot));
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Async method for deleting lot.
        /// </summary>
        /// <param name="lotId">The lot ID.</param>
        /// <returns>The Task.</returns>
        /// <exception cref="NotFoundException">Thrown if lot not found in DB.</exception>
        public async Task DeleteLotAsync(int lotId)
        {
            if (await _unitOfWork.Lots.GetAsync(lotId) == null)
                throw new NotFoundException("Lot not found.");
            _unitOfWork.Lots.Delete(lotId);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Async method for getting all lots with pagination.
        /// </summary>
        /// <param name="limit">Number of items.</param>
        /// <param name="offset">Items to skip.</param>
        /// <returns>The Task, containing collection of lots DTOs and total lots count.</returns>
        public async Task<(IEnumerable<LotDTO> Lots, int TotalCount)> GetAllLotsAsync(int limit, int offset)
        {
            var (lots, totalCount) = await _unitOfWork.Lots.GetAllAsync(limit, offset);
            return (lots.Select(x => Mapper.Map<Lot, LotDTO>(x)).ToList(), totalCount);
        }

        /// <summary>
        /// Async method for finding lots by keywords with pagination.
        /// </summary>
        /// <param name="keywords">The part or full lot name.</param>
        /// <param name="limit">Number of items.</param>
        /// <param name="offset">Items to skip.</param>
        /// <returns>The Task, containing collection of found lots DTOs and total found lots count.</returns>
        public async Task<(IEnumerable<LotDTO> Lots, int TotalCount)> FindLotsAsync(string keywords, int limit, int offset)
        {
            var (lots, totalCount) = await _unitOfWork.Lots.FindAsync(x => x.Name.Contains(keywords), limit, offset);
            return (lots.Select(x => Mapper.Map<Lot, LotDTO>(x)), totalCount);
        }

        /// <summary>
        /// Async method for finding lots by created user ID with pagination.
        /// </summary>
        /// <param name="userProfileId">The user profile ID.</param>
        /// <param name="limit">Number of items.</param>
        /// <param name="offset">Items to skip.</param>
        /// <returns>The Task, containing collection of found lots DTOs and total found lots count.</returns>
        public async Task<(IEnumerable<LotDTO> Lots, int TotalCount)> GetLotsByUserAsync(int userProfileId, int limit, int offset)
        {
            var (lots, totalCount) = await _unitOfWork.Lots.FindAsync(x => x.UserId == userProfileId, limit, offset);
            return (lots.Select(x => Mapper.Map<Lot, LotDTO>(x)), totalCount);
        }

        /// <summary>
        /// Async method for getting lot by ID.
        /// </summary>
        /// <param name="lotId">The lot ID.</param>
        /// <returns>The Task, containing lot DTO.</returns>
        public async Task<LotDTO> GetLotAsync(int lotId)
        {
            return Mapper.Map<Lot, LotDTO>(await _unitOfWork.Lots.GetAsync(lotId));
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
