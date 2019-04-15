using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineAuction.DAL.Entities;

namespace OnlineAuction.DAL.Interfaces.Repositories
{
    /// <summary>
    /// Interface for bids repository.
    /// </summary>
    public interface IBidsRepository : IRepository<Bid>
    {
        /// <summary>
        /// Method for fetching all bids by lot id.
        /// </summary>
        /// <param name="lotId">Id of lot.</param>
        Task<IEnumerable<Bid>> GetAllByLotAsync(int lotId);
        /// <summary>
        /// Method for fetching all bids by user profile id.
        /// </summary>
        /// <param name="userId">Id of user.</param>
        Task<IEnumerable<Bid>> GetAllByUserAsync(int userId);
    }
}
