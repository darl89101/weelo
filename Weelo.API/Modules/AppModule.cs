using Autofac;
using Weelo.Domain.Interfaces.Entities;

namespace Weelo.API.Modules
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new EFModule());
            builder.RegisterAssemblyTypes(assemblies)
               .AssignableTo<Inject>()
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
