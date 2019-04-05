using System.Threading.Tasks;
using OnlineAuction.DAL.Entities;

namespace OnlineAuction.DAL.Interfaces
{
    public interface IRoleManager
    {
        Task<ApplicationRole> FindByNameAsync(string roleName);
        Task<ApplicationRole> FindByIdAsync(string roleId);
    }
}
