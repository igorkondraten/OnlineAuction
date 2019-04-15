using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;
using Ninject.Modules;
using OnlineAuction.DAL.EF;
using OnlineAuction.DAL.Entities;
using OnlineAuction.DAL.Interfaces;
using Ninject.Web.Common;

namespace OnlineAuction.DAL.Infrastructure
{
    /// <summary>
    /// Data access layer module for Ninject.
    /// </summary>
    public class NinjectDALModule : NinjectModule
    {
        /// <summary>
        /// Connection string to DB.
        /// </summary>
        private readonly string _connection;

        /// <summary>
        /// Creates Ninject module with connection string.
        /// </summary>
        /// <param name="connection">Connection string to DB.</param>
        public NinjectDALModule(string connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Loads dependencies.
        /// </summary>
        public override void Load()
        {
            Bind<IDataContext>().To<AuctionContext>()
                .InRequestScope()
                .WithConstructorArgument(_connection);
            Bind<IRoleStore<ApplicationRole, string>>().To<RoleStore<ApplicationRole>>()
                .InRequestScope()
                .WithConstructorArgument("context", context => context.Kernel.Get<IDataContext>());
            Bind<IUserStore<ApplicationUser>>().To<UserStore<ApplicationUser>>()
                .InRequestScope()
                .WithConstructorArgument("context", context => context.Kernel.Get<IDataContext>());
        }
    }
}
