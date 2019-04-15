using Microsoft.AspNet.Identity;
using OnlineAuction.DAL.Entities;
using OnlineAuction.DAL.Interfaces;

namespace OnlineAuction.DAL.Identity
{
    /// <summary>
    /// Manager for identity roles.
    /// </summary>
    public class ApplicationRoleManager : RoleManager<ApplicationRole>, IRoleManager
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> store) : base(store)
        {
        }
    }
}
