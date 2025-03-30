using Constructor_API.Application.Services;
using Constructor_API.Core.Repositories;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Infractructure;
using Constructor_API.Infractructure.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;


var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        var serializerOptions = opt.JsonSerializerOptions;
        serializerOptions.IgnoreNullValues = true;
        serializerOptions.IgnoreReadOnlyProperties = false;
        serializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<MongoDBContext>();

builder.Services.AddScoped<IBuildingRepository, BuildingMongoRepository>();
builder.Services.AddScoped<IFloorRepository, FloorMongoRepository>();
builder.Services.AddScoped<IGraphPointRepository, GraphPointMongoRepository>();
builder.Services.AddScoped<IFloorConnectionRepository, FloorConnectionMongoRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectMongoRepository>();
builder.Services.AddScoped<IPredefinedGraphPointTypeRepository, PredefinedGraphPointTypeMongoRepository>();
builder.Services.AddScoped<IPredefinedCategoryRepository, PredefinedCategoryMongoRepository>();
builder.Services.AddScoped<IUserRepository, UserMongoRepository>();

builder.Services.AddScoped<FloorConnectionService>();
builder.Services.AddScoped<GraphPointService>();
builder.Services.AddScoped<FloorService>();
builder.Services.AddScoped<BuildingService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

app.UseExceptionHandler(builder =>
{
    builder.Run(async context =>
    {
        var err = context.Features.Get<IExceptionHandlerFeature>().Error;
        context.Response.ContentType = "application/problem+json";
        //err.

        if (err is ValidationException valException)
        {
            //await context.Handler400ExceptionAsync(valException);
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(valException.Message);
            return;
        }
        else if (err is AlreadyExistsException existsException)
        {
            //await context.Handler400ExceptionAsync(existsException);
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(existsException.Message);
            return;
        }
        else if (err is NotFoundException notFoundException)
        {
            //await context.Handler404ExceptionAsync(notFoundException);
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(notFoundException.Message);
            return;
        }


        //await context.Handler500ExceptionAsync(err);
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync(err.Message);
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
