using Ninject.Modules;
using Ninject.Web.Common;
using OnlineAuction.BLL.Interfaces;
using OnlineAuction.BLL.Services;
using OnlineAuction.DAL.Infrastructure;
using OnlineAuction.DAL.Interfaces;
using OnlineAuction.DAL.Repositories;


namespace OnlineAuction.BLL.Infrastructure
{
    /// <summary>
    /// Business logic layer module for Ninject.
    /// </summary>
    public class NinjectBLLModule : NinjectModule
    {
        /// <summary>
        /// DAL Ninject module.
        /// </summary>
        private NinjectDALModule _DALModule;

        /// <summary>
        /// Creates Ninject module with connection string.
        /// </summary>
        /// <param name="connection">Connection string to DB.</param>
        public NinjectBLLModule(string connection)
        {
            _DALModule = new NinjectDALModule(connection);
        }

        /// <summary>
        /// Loads dependencies.
        /// </summary>
        public override void Load()
        {
            Kernel?.Load(new INinjectModule[] { _DALModule });
            Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            Bind<IBidsService>().To<BidsService>().InRequestScope();
            Bind<ILotsService>().To<LotsService>().InRequestScope();
            Bind<ICategoriesService>().To<CategoriesService>().InRequestScope();
            Bind<IUsersService>().To<UsersService>().InRequestScope();
        }
    }
}
