using FluentValidation;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Queries;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using HospitalManagementSystem.Api.Repositories;
using HospitalManagementSystem.Api.Validators;
using HospitalManagementSystem.Infra.MongoDBStructure.Config;
using HospitalManagementSystem.Infra.MongoDBStructure.Interfaces;
using HospitalManagementSystem.Infra.MongoDBStructure;
using Serilog;
using System.Reflection;
using MediatR;
using FluentValidation.AspNetCore;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Api
{
    public class Startup
    {
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        //public IConfiguration Configuration { get; }

        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddControllers();
        //    services.AddSingleton<MongoConfig>();
        //    services.AddSingleton(typeof(Serilog.ILogger), _ => Log.Logger);
        //    services.AddSingleton<IMongoFactory, MongoFactory>();
        //    services.AddSingleton<IReadStore, ReadStore>();
        //    services.AddMediatR(typeof(Program).GetTypeInfo().Assembly);
        //    services.AddSingleton<IDoctorsRepository, DoctorsRepository>();
        //    services.AddTransient<IValidator<DoctorsQuery>, DoctorsQueryValidator>();
        //    services.AddEndpointsApiExplorer();
        //    services.AddSwaggerGen();
        //    // Add Fluent Validation to the services container
        //    services.AddMvc()
        //        .AddFluentValidation(options =>
        //        {
        //            // Configure Fluent Validation to use the MyValidator class
        //            options.RegisterValidatorsFromAssemblyContaining<DoctorsQueryValidator>();
        //        });
        //}

        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        //{

        //    app.UseSwagger();
        //    app.UseSwaggerUI();

        //    app.UseHttpsRedirection();

        //    app.UseAuthorization();
        //}
    }
}
