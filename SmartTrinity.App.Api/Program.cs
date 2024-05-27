using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartTrinity.App.Api.Middlewares;
using SmartTrinity.App.Api.Services;
using SmartTrinity.App.Core.Communication;
using SmartTrinity.App.Core.Services;
using SmartTrinity.App.Migrations.Core.Repositories;
using SmartTrinity.App.Migrations.Infrastructure;
using SmartTrinity.App.Migrations.Infrastructure.Repositories;
using SmartTrinity.App.Sales.Infrastructure.Repositories;
using SmartTrinity.App.Sales.Services;
using SmartTrinity.App.Services;
using SmartTrinity.Core.Database;
using SmartTrinity.Core.Models;
using SmartTrinity.Core.Services;
using SmartTrinity.Infrastructure.Database;
using SmartTrinity.Infrastructure.Database.UnitOfWork;
using SmartTrinity.Shared.Services;
using System.Reflection;
using System.Text;
using MigrationServices = SmartTrinity.App.Migrations.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    var key = Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Secret"]);
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

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

//DI Setup
//Hack: Create a extension method for inject all dependencies related to shared services.
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddScoped(typeof(ICustomerUnitOfWork), typeof(CustomerUnitOfWork));
builder.Services.AddScoped(typeof(ICustomerRepository), typeof(CustomerRepository));
builder.Services.AddTransient<ISalesService, SalesService>();
builder.Services.AddTransient(typeof(MigrationServices.ISalesService),typeof( MigrationServices.SalesService));
builder.Services.AddScoped<ISalesUnitOfWork, SalesUnitOfWork>();


//Services:
builder.Services.AddTransient<IUsersService, UsersService>();

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

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

app.UsePathBase(new PathString("/api"));
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
