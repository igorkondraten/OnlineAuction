using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineAuction.BLL.DTO;

namespace OnlineAuction.BLL.Interfaces
{
    /// <summary>
    /// Interface for lots service.
    /// Contains methods for managing lots.
    /// </summary>
    public interface ILotsService : IDisposable
    {
        /// <summary>
        /// Async method for creating lot.
        /// </summary>
        /// <param name="lot">The lot DTO.</param>
        /// <returns>The Task, containing created lot DTO.</returns>
        Task<LotDTO> CreateLotAsync(LotDTO lot);

        /// <summary>
        /// Async method for updating lot.
        /// </summary>
        /// <param name="lot">The lot DTO.</param>
        /// <returns>The Task.</returns>
        Task EditLotAsync(LotDTO lot);

        /// <summary>
        /// Async method for deleting lot.
        /// </summary>
        /// <param name="lotId">The lot ID.</param>
        /// <returns>The Task.</returns>
        Task DeleteLotAsync(int lotId);

        /// <summary>
        /// Async method for getting all lots with pagination.
        /// </summary>
        /// <param name="limit">Number of items.</param>
        /// <param name="offset">Items to skip.</param>
        /// <returns>The Task, containing collection of lots DTOs and total lots count.</returns>
        Task<(IEnumerable<LotDTO> Lots, int TotalCount)> GetAllLotsAsync(int limit, int offset);

        /// <summary>
        /// Async method for finding lots by keywords with pagination.
        /// </summary>
        /// <param name="keywords">The part or full lot name.</param>
        /// <param name="limit">Number of items.</param>
        /// <param name="offset">Items to skip.</param>
        /// <returns>The Task, containing collection of found lots DTOs and total found lots count.</returns>
        Task<(IEnumerable<LotDTO> Lots, int TotalCount)> FindLotsAsync(string keywords, int limit, int offset);

        /// <summary>
        /// Async method for finding lots by created user ID with pagination.
        /// </summary>
        /// <param name="userProfileId">The user profile ID.</param>
        /// <param name="limit">Number of items.</param>
        /// <param name="offset">Items to skip.</param>
        /// <returns>The Task, containing collection of found lots DTOs and total found lots count.</returns>
        Task<(IEnumerable<LotDTO> Lots, int TotalCount)> GetLotsByUserAsync(int userProfileId, int limit, int offset);

        /// <summary>
        /// Async method for getting lot by ID.
        /// </summary>
        /// <param name="lotId">The lot ID.</param>
        /// <returns>The Task, containing lot DTO.</returns>
        Task<LotDTO> GetLotAsync(int lotId);
    }
}
