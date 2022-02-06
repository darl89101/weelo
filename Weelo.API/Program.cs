using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Weelo.API.Abstract;
using Weelo.Domain.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Configure autofac modules and context

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterType<Weelo.DataAccess.AppContext>().AsSelf().As<DbContext>()
            .WithParameter("options", new DbContextOptionsBuilder<DbContext>()
            .UseSqlServer(builder.Configuration.GetConnectionString("Cnx"), serverDbContextOptionsBuilder =>
                serverDbContextOptionsBuilder.CommandTimeout(120)).Options)
            .InstancePerLifetimeScope();
    container.RegisterAssemblyModules(typeof(Program).Assembly);
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddMvc().AddMvcOptions(opt => opt.ModelValidatorProviders.Clear())
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

// Add json serializer

builder.Services.AddMvc().AddNewtonsoftJson(opt =>
{
    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    opt.SerializerSettings.Converters.Add(new StringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCustomSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseCustomSwagger();
}
else app.UseMiddleware<ExceptionMiddlewareProduction>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
