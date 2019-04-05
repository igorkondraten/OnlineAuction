using Ninject.Modules;
using Ninject.Web.Common;
using OnlineAuction.BLL.Interfaces;
using OnlineAuction.BLL.Services;
using OnlineAuction.DAL.Infrastructure;
using OnlineAuction.DAL.Interfaces;
using OnlineAuction.DAL.Repositories;


namespace OnlineAuction.BLL.Infrastructure
{
    public class NinjectBLLModule : NinjectModule
    {
        private NinjectDALModule _DALModule;

        public NinjectBLLModule(string connection)
        {
            _DALModule = new NinjectDALModule(connection);
        }
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
