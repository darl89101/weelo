using Microsoft.OpenApi.Models;

namespace Weelo.API.Abstract
{
    /// <summary>
    /// Extensions for the API
    /// </summary>
    public static class APIExtensions
    {
        /// <summary>
        /// Configuración del swagger
        /// </summary>
        /// <param name="services"></param>
        public static void AddCustomSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(
                   name: "v1",
                   info: new OpenApiInfo()
                   {
                       Title = "App Core API",
                       Version = "v1",
                       Description = "Through this API you can access the site's capabilities.",
                       Contact = new OpenApiContact()
                       {
                           Email = "name@site.com",
                           Name = "DNT",
                           Url = new Uri("http://www.dotnettips.info")
                       },
                       License = new OpenApiLicense()
                       {
                           Name = "MIT License",
                           Url = new Uri("https://opensource.org/licenses/MIT")
                       }
                   }
                );

                var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
                xmlFiles.ForEach(xmlFile => setupAction.IncludeXmlComments(xmlFile));
            });
        }

        /// <summary>
        /// Use Custom Swagger
        /// </summary>
        /// <param name="app"></param>
        public static void UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint(
                    url: "/swagger/v1/swagger.json",
                    name: "v1");

                setupAction.DefaultModelExpandDepth(2);
                setupAction.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
                setupAction.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                setupAction.EnableDeepLinking();
                setupAction.DisplayOperationId();
            });
        }
    }
}
