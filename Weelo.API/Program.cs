using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Weelo.API.Abstract;
using Weelo.Domain.Middlewares;
using Weelo.Domain.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure autofac modules and context

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterType<Weelo.DataAccess.AppContext>().AsSelf().As<DbContext>()
            .WithParameter("options", new DbContextOptionsBuilder<IdentityDbContext<IdentityUser>>()
            .UseSqlServer(builder.Configuration.GetConnectionString("Cnx"), serverDbContextOptionsBuilder =>
                serverDbContextOptionsBuilder.CommandTimeout(120)).Options)
            .InstancePerLifetimeScope();
    container.RegisterAssemblyModules(typeof(Program).Assembly);
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddCustomAuthentication(builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>());
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddMvc().AddMvcOptions(opt => opt.ModelValidatorProviders.Clear())
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());
builder.Services.AddCustomIdentityCore(builder.Configuration.GetConnectionString("Cnx"));
builder.Services.AddAuthorization();

// Add json serializer

builder.Services.AddMvc().AddNewtonsoftJson(opt =>
{
    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    opt.SerializerSettings.Converters.Add(new StringEnumConverter());
});

builder.Services.AddCustomSwagger();

var app = builder.Build();
app.UseAuthentication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseCustomSwagger();
}
else app.UseMiddleware<ExceptionMiddlewareProduction>();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
