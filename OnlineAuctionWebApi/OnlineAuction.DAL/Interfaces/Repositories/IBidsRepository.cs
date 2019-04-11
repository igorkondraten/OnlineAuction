using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineAuction.DAL.Entities;

namespace OnlineAuction.DAL.Interfaces.Repositories
{
    public interface IBidsRepository : IRepository<Bid>
    {
        Task<IEnumerable<Bid>> GetAllByLotAsync(int lotId);
        Task<IEnumerable<Bid>> GetAllByUserAsync(int userId);
    }
}
