using Constructor_API.Application.Authorization.Handlers;
using Constructor_API.Application.Authorization.Requirements;
using Constructor_API.Application.Services;
using Constructor_API.Core.Repositories;
using Constructor_API.Helpers.Exceptions;
using Constructor_API.Infractructure;
using Constructor_API.Infractructure.Repositories;
using Constructor_API.Models.Entities;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
//using static System.Net.Mime.MediaTypeNames;


var builder = WebApplication.CreateBuilder(args);
Env.Load();
//Добавление файла конфигурации
builder.Configuration.AddEnvironmentVariables();

var testOrigins = "testOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: testOrigins,
                      policy =>
                      {
                          policy
                          .AllowAnyOrigin()
                          //.WithOrigins(builder.Configuration["TestOrigin1"],
                          //    builder.Configuration["TestOrigin2"])
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                          //.AllowCredentials();
                      });
});


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

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        //Description = "Please enter token in format \"bearer *token*\"",
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<MongoDBContext>();

builder.Services.AddScoped<IBuildingRepository, BuildingMongoRepository>();
builder.Services.AddScoped<IFloorRepository, FloorMongoRepository>();
builder.Services.AddScoped<IGraphPointRepository, GraphPointMongoRepository>();
builder.Services.AddScoped<IFloorsTransitionRepository, FloorsTransitionMongoRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectMongoRepository>();
builder.Services.AddScoped<IPredefinedGraphPointTypeRepository, PredefinedGraphPointTypeMongoRepository>();
builder.Services.AddScoped<IPredefinedCategoryRepository, PredefinedCategoryMongoRepository>();
builder.Services.AddScoped<IUserRepository, UserMongoRepository>();
builder.Services.AddScoped<IProjectUserRepository, ProjectUserMongoRepository>();

builder.Services.AddScoped<FloorsTransitionService>();
builder.Services.AddScoped<GraphPointService>();
builder.Services.AddScoped<FloorService>();
builder.Services.AddScoped<BuildingService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PathService>();


builder.Services.AddScoped<IAuthorizationHandler, UserAuthorizationHandler>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    //Только dev
    options.RequireHttpsMetadata = false;
    //options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Secret"])),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true, 
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Project", policy =>
        policy.Requirements.Add(new TypeRequirement("Project")));

    options.AddPolicy("Building", policy =>
        policy.Requirements.Add(new TypeRequirement("Building")));

    options.AddPolicy("Floor", policy =>
        policy.Requirements.Add(new TypeRequirement("Floor")));

    options.AddPolicy("GraphPoint", policy =>
        policy.Requirements.Add(new TypeRequirement("GraphPoint")));

    options.AddPolicy("FloorConnection", policy =>
        policy.Requirements.Add(new TypeRequirement("FloorConnection")));
});

var app = builder.Build();

app.UseCors(testOrigins);

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
