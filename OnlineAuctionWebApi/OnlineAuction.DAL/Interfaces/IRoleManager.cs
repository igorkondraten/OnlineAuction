using System.Threading.Tasks;
using OnlineAuction.DAL.Entities;

namespace OnlineAuction.DAL.Interfaces
{
    /// <summary>
    /// Interface for managing roles.
    /// </summary>
    public interface IRoleManager
    {
        /// <summary>
        /// Gets role by role name.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns>The Task, containing the ApplicationRole object.</returns>
        Task<ApplicationRole> FindByNameAsync(string roleName);

        /// <summary>
        /// Gets role by role id.
        /// </summary>
        /// <param name="roleId">Id of the role.</param>
        /// <returns>The Task, containing the ApplicationRole object.</returns>
        Task<ApplicationRole> FindByIdAsync(string roleId);
    }
}
