using Autofac;
using Weelo.DataAccess.Repositories.Base;
using Weelo.Domain.Interfaces.Base;

namespace Weelo.API.Modules
{
    public class EFModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(UnitOfWork)).As(typeof(IUnitOfWork)).InstancePerLifetimeScope();
        }
    }
}
