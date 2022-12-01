using HospitalManagementSystem.Api.Models;
using HospitalManagementSystem.Api.Repositories;
using HospitalManagementSystem.Api.Repositories.Interfaces;
using HospitalManagementSystem.Infra.MongoDBStructure;
using HospitalManagementSystem.Infra.MongoDBStructure.Config;
using HospitalManagementSystem.Infra.MongoDBStructure.Interfaces;
using MongoDB.Driver;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<HospitalManagementSystemDatabaseSettings>(
    builder.Configuration.GetSection("HospitalManagementSystemDatabase"));
builder.Services.AddSingleton<MongoConfig>();
builder.Services.AddSingleton(typeof(Serilog.ILogger), _ => Log.Logger);
builder.Services.AddSingleton<IDoctorsRepository, DoctorsRepository>();
builder.Services.AddSingleton<IMongoFactory, MongoFactory>();
builder.Services.AddSingleton<IReadStore, ReadStore>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
