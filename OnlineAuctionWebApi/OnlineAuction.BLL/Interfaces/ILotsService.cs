using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineAuction.BLL.DTO;

namespace OnlineAuction.BLL.Interfaces
{
    public interface ILotsService : IDisposable
    {
        Task<LotDTO> CreateLotAsync(LotDTO lot);
        Task EditLotAsync(LotDTO lot);
        Task DeleteLotAsync(int lotId);
        Task<(IEnumerable<LotDTO> Lots, int TotalCount)> GetAllLotsAsync(int limit, int offset);
        Task<(IEnumerable<LotDTO> Lots, int TotalCount)> FindLotsAsync(string keywords, int limit, int offset);
        Task<(IEnumerable<LotDTO> Lots, int TotalCount)> GetLotsByUserAsync(int userId, int limit, int offset);
        Task<LotDTO> GetLotAsync(int lotId);
    }
}
