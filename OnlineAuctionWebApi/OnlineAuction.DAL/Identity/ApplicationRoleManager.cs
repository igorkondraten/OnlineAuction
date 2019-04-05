using Microsoft.AspNet.Identity;
using OnlineAuction.DAL.Entities;
using OnlineAuction.DAL.Interfaces;

namespace OnlineAuction.DAL.Identity
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>, IRoleManager
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> store) : base(store)
        {
        }
    }
}
