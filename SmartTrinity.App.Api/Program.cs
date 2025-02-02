using System.Text;
using Asp.Versioning;
using System.Reflection;
using Microsoft.OpenApi.Models;
using SmartTrinity.Core.Models;
using SmartTrinity.App.Services;
using SmartTrinity.Core.Database;
using SmartTrinity.Core.Services;
using SmartTrinity.Shared.Services;
using SmartTrinity.App.Core.Models;
using SmartTrinity.App.Api.Services;
using Microsoft.IdentityModel.Tokens;
using SmartTrinity.App.Core.Services;
using SmartTrinity.App.Pumps.Services;
using SmartTrinity.App.Api.Middlewares;
using SmartTrinity.App.Migrations.Core;
using SmartTrinity.Shared.Core.Services;
using SmartTrinity.App.Core.Communication;
using SmartTrinity.Infrastructure.Database;
using SmartTrinity.App.Pumps.Core.Services;
using SmartTrinity.App.Pumps.Infrastructure;
using SmartTrinity.App.Migrations.Core.Models;
using SmartTrinity.App.Migrations.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SmartTrinity.Infrastructure.Database.UnitOfWork;
using SmartTrinity.App.Sales.Infrastructure.Repositories;
using SmartTrinity.Shared.Infrastructure.Database;
using SmartTrinity.Shared.Core.Database;
using SmartTrinity.App.Migrations.Core.Services;
using SmartTrinity.App.Migrations.Services;
using SmartTrinity.App.Pumps.Core.Database;
using SmartTrinity.App.FuelStation.Infrastructure;
using SmartTrinity.App.FuelStation.Core.Database;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(builder.Configuration.GetSection("Clients:Frontend").Get<string[]>());
            policy.AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();

            policy.WithOrigins(builder.Configuration.GetSection("Clients:Backend").Get<string[]>());
            policy.AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    var key = Encoding.UTF8.GetBytes(builder.Configuration[key: "AppSettings:Secret"]);
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

//Settings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<ConsoleSettings>(builder.Configuration.GetSection("ConsoleSettings"));
builder.Services.Configure<MigratorSettings>(builder.Configuration.GetSection("MigratorSettings"));

builder.Services.AddSignalR();

//Hosted service
builder.Services.AddHostedService<TrinityBackgroundService>();
//builder.Services.AddTransient<ISalesMigrator, SalesMigrator>();

//DI Setup
//Hack: Create a extension method for inject all dependencies related to shared services.
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<ISalesUnitOfWork, SalesUnitOfWork>();
builder.Services.AddTransient<IPumpsUnitOfWork, PumpsUnitOfWork>();
builder.Services.AddTransient<IStationUnitOfWork, StationUnitOfWork>();
builder.Services.AddTransient<ISalesMigratorUnitOfWork, SalesMigratorUnitOfWork>();

//Services:
builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddTransient<ISalesService, SalesService>();
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddTransient<IPumpsService, PumpsService>();
builder.Services.AddTransient<IMessageService, MessageService>();

//Test services:
builder.Services.AddTransient<IPumpsTestService, PumpsTestService>();

//ComunicationManager
builder.Services.AddSingleton<ICommunicationManager, CommunicateManagerService>();
builder.Services.AddSingleton<ISmartTrinityService, SmartTrinityService>();
builder.Services.AddSwaggerGen(options =>
{
    // Set the comments path for the Swagger JSON and UI.**
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SmartTrinity APIs",
        Version = "v1.0",
        Description = "REST APIs ",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

    // To Enable authorization using Swagger (JWT)
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

//Database setup
DapperDatabaseManager.Setup();

app.UseCors(MyAllowSpecificOrigins);

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

app.MapHub<SmartTrinityHubService>("/smartTrinityHub");

app.UsePathBase(new PathString("/api"));
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
