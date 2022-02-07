using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Weelo.Domain.Exceptions;
using Weelo.Domain.Models;
using IdentityRole = Microsoft.AspNetCore.Identity.IdentityRole;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;

namespace Weelo.API.Abstract
{
    /// <summary>
    /// Extensions for the API
    /// </summary>
    public static class APIExtensions
    {
        /// <summary>
        /// Add jwt configuration
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="NotAuthorizeException"></exception>
        /// <exception cref="BadRequestException"></exception>
        public static void AddCustomAuthentication(this IServiceCollection services, JwtOptions jwtOptions)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(cfg =>
               {
                   cfg.RequireHttpsMetadata = false;
                   cfg.SaveToken = true;

                   cfg.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidIssuer = jwtOptions.Issuer,
                       ValidateIssuer = false,
                       ValidAudience = jwtOptions.Audience,
                       ValidateAudience = false,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ClockSkew = TimeSpan.Zero, // tolerance for the expiration date
                       LifetimeValidator = LifetimeValidator
                   };

                   cfg.Events = new()
                   {
                       OnAuthenticationFailed = context =>
                       {
                           var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                           return Task.CompletedTask;
                       },
                       OnTokenValidated = context =>
                       {
                           return Task.CompletedTask;
                       },
                       OnMessageReceived = context =>
                       {
                           return Task.CompletedTask;
                       },
                       OnChallenge = context =>
                       {
                           var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));

                           if (context.AuthenticateFailure != null) throw new NotAuthorizeException(context.AuthenticateFailure.Message);
                           else if (!string.IsNullOrEmpty(context.Error)) throw new BadRequestException(context.Error);
                           return Task.CompletedTask;
                       }
                   };
               });
        }

        /// <summary>
        /// Valida el token
        /// </summary>
        /// <param name="notBefore"></param>
        /// <param name="expires"></param>
        /// <param name="securityToken"></param>
        /// <param name="validationParameters"></param>
        /// <returns></returns>
        private static bool LifetimeValidator(DateTime? notBefore,
                                      DateTime? expires,
                                      SecurityToken securityToken,
                                      TokenValidationParameters validationParameters)
            => (expires.HasValue && DateTime.UtcNow < expires)
                && (notBefore.HasValue && DateTime.UtcNow > notBefore);

        /// <summary>
        /// add identity server
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        public static void AddCustomIdentityCore(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DataAccess.AppContext>(options =>
            options.UseSqlServer(connectionString, serverDbContextOptionsBuilder =>
            {
                var minutes = (int)TimeSpan.FromMinutes(2).TotalSeconds;
                serverDbContextOptionsBuilder.CommandTimeout(minutes);
                serverDbContextOptionsBuilder.EnableRetryOnFailure();
            }));

            services.AddIdentityCore<IdentityUser>(config =>
            {
                config.SignIn.RequireConfirmedEmail = false;
                config.User.RequireUniqueEmail = true;
                config.Password = new PasswordOptions
                {
                    RequiredLength = 6,
                    RequireDigit = true,
                };
            })
                .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddEntityFrameworkStores<DataAccess.AppContext>()
            .AddDefaultTokenProviders();
        }

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

                setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,//Http
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string> { "Bearer" }
                    }
                });
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
