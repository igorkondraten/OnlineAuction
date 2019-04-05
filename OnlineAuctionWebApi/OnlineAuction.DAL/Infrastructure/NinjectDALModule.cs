using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;
using Ninject.Extensions.NamedScope;
using Ninject.Modules;
using OnlineAuction.DAL.EF;
using OnlineAuction.DAL.Entities;
using OnlineAuction.DAL.Interfaces;
using Ninject.Web.Common;

namespace OnlineAuction.DAL.Infrastructure
{
    public class NinjectDALModule : NinjectModule
    {
        private readonly string _connection;

        public NinjectDALModule(string connection)
        {
            _connection = connection;
        }
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
