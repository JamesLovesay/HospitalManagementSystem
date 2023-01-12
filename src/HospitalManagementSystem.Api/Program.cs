using FluentValidation;
using FluentValidation.AspNetCore;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Queries;
using HospitalManagementSystem.Api.Repositories;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using HospitalManagementSystem.Api.Validators;
using HospitalManagementSystem.Infra.MongoDBStructure;
using HospitalManagementSystem.Infra.MongoDBStructure.Config;
using HospitalManagementSystem.Infra.MongoDBStructure.Interfaces;
using MediatR;
using Serilog;
using System.Reflection;
using HospitalManagementSystem.Api;
using Microsoft.AspNetCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using MediatR.Pipeline;
using HospitalManagementSystem.Api.Controllers;
using HospitalManagementSystem.Api.Commands;
using HospitalManagementSystem.Api.Handlers;

public class Program
{
    private static void Main(string[] args)
    {
        //CreateWebHostBuilder(args).Build().Run();

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.Configure<HospitalManagementSystemDatabaseSettings>(
        builder.Configuration.GetSection("HospitalManagementSystemDatabase"));
        builder.Services.AddSingleton<MongoConfig>();
        builder.Services.AddSingleton(typeof(Serilog.ILogger), _ => Log.Logger);
        builder.Services.AddSingleton<IMongoFactory, MongoFactory>();
        builder.Services.AddSingleton<IReadStore, ReadStore>();
        builder.Services.AddMediatR(typeof(Program).GetTypeInfo().Assembly);
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
        builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddMediatR(typeof(DoctorsQueryHandler).GetTypeInfo().Assembly);
        builder.Services.AddMediatR(typeof(DoctorsQueryHandler));
        builder.Services.AddMediatR(typeof(CreateDoctorCommand).GetTypeInfo().Assembly);
        builder.Services.AddMediatR(typeof(CreateDoctorCommand));
        builder.Services.AddMediatR(typeof(DoctorDeleteCommand).GetTypeInfo().Assembly);
        builder.Services.AddMediatR(typeof(CreateDoctorCommandHandler).GetTypeInfo().Assembly);
        builder.Services.AddMediatR(typeof(DeleteDoctorCommandHandler).GetTypeInfo().Assembly);
        builder.Services.AddTransient<Mediator>();
        builder.Services.AddSingleton<IDoctorsRepository, DoctorsRepository>();
        builder.Services.AddScoped<IValidator<DoctorsQuery>, DoctorsQueryValidator>();
        builder.Services.AddTransient<IValidator<DoctorsQuery>, DoctorsQueryValidator>();
        builder.Services.AddTransient<IValidator<CreateDoctorCommand>, CreateDoctorValidator>();
        builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                }); ;
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}