using Autofac;
using Weelo.DataAccess.Repositories.Base;
using Weelo.Domain.Interfaces.Base;

namespace Weelo.API.Modules
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {            
            builder.RegisterGeneric(typeof(GenericRepository<,>))
                .As(typeof(IGenericRepository<,>)).InstancePerLifetimeScope();
        }
    }
}
