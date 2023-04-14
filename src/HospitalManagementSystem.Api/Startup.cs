using FluentValidation;
using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using HospitalManagementSystem.Api.Repositories;
using HospitalManagementSystem.Infra.MongoDBStructure.Config;
using HospitalManagementSystem.Infra.MongoDBStructure.Interfaces;
using HospitalManagementSystem.Infra.MongoDBStructure;
using Serilog;
using MediatR;
using MediatR.Pipeline;
using System.Text.Json.Serialization;
using HospitalManagementSystem.Api.Commands.Doctors;
using HospitalManagementSystem.Api.Handlers.Doctors;
using HospitalManagementSystem.Api.Queries.Doctors;
using HospitalManagementSystem.Api.Validators.Doctors;
using HospitalManagementSystem.Api.Events.Appointments;
using System;

namespace HospitalManagementSystem.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = WebApplication.CreateBuilder();
            var configuration = builder.Configuration;

            services.Configure<HospitalManagementSystemDatabaseSettings>(
            configuration.GetSection("HospitalManagementSystemDatabase"));
            services.AddSingleton<MongoConfig>();
            services.AddSingleton(typeof(Serilog.ILogger), _ => Log.Logger);
            services.AddSingleton<IMongoFactory, MongoFactory>();
            services.AddSingleton<IReadStore, ReadStore>();
            services.AddMediatR(typeof(DoctorsQueryHandler).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddMediatR(typeof(CreateDoctorCommand).Assembly);
            services.AddMediatR(typeof(DoctorDeleteCommand).Assembly);
            services.AddMediatR(typeof(CreateDoctorCommandHandler).Assembly);
            services.AddMediatR(typeof(DeleteDoctorCommandHandler).Assembly);
            services.AddTransient<IEventHandler<AppointmentCreatedEvent>, AppointmentCreatedEventHandler>();
            services.AddTransient<Mediator>();
            services.AddSingleton<IDoctorsRepository, DoctorsRepository>();
            services.AddTransient<IValidator<DoctorsQuery>, DoctorsQueryValidator>();
            services.AddTransient<IValidator<CreateDoctorCommand>, CreateDoctorValidator>();
            services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    }); ;
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
