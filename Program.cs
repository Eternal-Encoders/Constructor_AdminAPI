using ConstructorAdminAPI.Application.Services;
using ConstructorAdminAPI.Core.Repositories;
using ConstructorAdminAPI.Infractructure;
using ConstructorAdminAPI.Infractructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
    .AddJsonOptions(opt =>
    {
        var serializerOptions = opt.JsonSerializerOptions;
        serializerOptions.IgnoreNullValues = true;
        serializerOptions.IgnoreReadOnlyProperties = false;
        serializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<MongoDBContext>();
builder.Services.AddScoped<FloorService>();
builder.Services.AddScoped<IFloorRepository, FloorMongoRepository>();
builder.Services.AddScoped<IGraphPointRepository, GraphPointMongoRepository>();
builder.Services.AddScoped<IStairRepository, StairMongoRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
