using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using Weelo.API.Modules;
using Weelo.API.Profiles;

namespace Weelo.tests
{
    internal abstract class BaseTest
    {
        /// <summary>
        /// Obtains the configuration root
        /// </summary>
        /// <returns></returns>
        public IConfigurationRoot GetConfigurationRoot()
            => new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile(Directory.GetCurrentDirectory() + "./appsettings.json")
               .Build();

        /// <summary>
        /// Register autofact profiles
        /// </summary>
        /// <param name="container"></param>
        public void RegisterProfiles(ContainerBuilder container)
        {
            container.Register(context => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<OwnerProfile>();
                cfg.AddProfile<PropertyProfile>();
                cfg.AddProfile<PropertyTraceProfile>();
                cfg.AddProfile<UpdateModelProfile>();
            })).AsSelf().SingleInstance();
        }

        /// <summary>
        /// Register automapper
        /// </summary>
        /// <param name="container"></param>
        public void RegisterAutoMapper(ContainerBuilder container)
        {
            container.Register(c =>
            {
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper(context.Resolve);
            })
             .As<IMapper>()
             .InstancePerLifetimeScope();
        }

        /// <summary>
        /// Get Container
        /// </summary>
        /// <returns></returns>
        public IContainer GetContainer()
        {
            var configuration = GetConfigurationRoot();
            var cnx = configuration.GetConnectionString("Cnx");
            var container = new ContainerBuilder();
            container.RegisterType<DataAccess.AppContext>().AsSelf().As<DbContext>()
            .WithParameter("options", new DbContextOptionsBuilder<DbContext>()
            .UseSqlServer(cnx, serverDbContextOptionsBuilder =>
                serverDbContextOptionsBuilder.CommandTimeout(120)).Options)
            .InstancePerLifetimeScope();
            container.RegisterAssemblyModules(typeof(AppModule).Assembly);
            RegisterProfiles(container);
            RegisterAutoMapper(container);
            container.RegisterInstance(configuration).As<IConfiguration>();
            return container.Build();
        }

        /// <summary>
        /// Resolve a dependency
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>() => GetContainer().Resolve<T>();
    }
}
