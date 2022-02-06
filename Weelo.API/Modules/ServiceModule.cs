using Autofac;

namespace Weelo.API.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterGeneric(typeof(ServiceBase<,>))
            //    .As(typeof(IServiceBase<,>)).InstancePerLifetimeScope();
        }
    }
}
