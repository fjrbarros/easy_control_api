using System.Text;
using AutoMapper;
using EasyControl.Api.AutoMapper;
using EasyControl.Api.Data;
using EasyControl.Api.Domain.Repository.Classes;
using EasyControl.Api.Domain.Repository.Interfaces;
using EasyControl.Api.Domain.Services.Classes;
using EasyControl.Api.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder);

ConfigureDependencyInjection(builder);

var app = builder.Build();

ConfigureApplication(app);

app.Run();

static void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services
        .AddCors()
        .AddControllers()
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        })
        .AddNewtonsoftJson();

    builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition(
            "Bearer",
            new OpenApiSecurityScheme
            {
                Description =
                    "JTW Authorization header using the Beaerer scheme (Example: 'Bearer 12345abcdef')",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            }
        );

        c.AddSecurityRequirement(
            new OpenApiSecurityRequirement
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
                    Array.Empty<string>()
                }
            }
        );

        c.SwaggerDoc("v1", new OpenApiInfo { Title = "EasyControll.Api", Version = "v1" });
    });

    builder.Services
        .AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(builder.Configuration["KeySecret"] ?? "")
                ),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
}

static void ConfigureDependencyInjection(WebApplicationBuilder builder)
{
    string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
    {
        connectionString = builder.Configuration.GetConnectionString("DockerConnection");
    }

    builder.Services.AddDbContext<ApplicationContext>(
        options => options.UseNpgsql(connectionString),
        ServiceLifetime.Transient,
        ServiceLifetime.Transient
    );

    var config = new MapperConfiguration(cfg =>
    {
        cfg.AddProfile<UserProfile>();
    });

    IMapper mapper = config.CreateMapper();

    builder.Services
        .AddSingleton(builder.Configuration)
        .AddSingleton(builder.Environment)
        .AddSingleton(mapper)
        .AddScoped<TokenService>()
        .AddScoped<IUserRepository, UserRepository>()
        .AddScoped<IUserService, UserService>();
}

static void ConfigureApplication(WebApplication app)
{
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

    app.UseDeveloperExceptionPage().UseRouting();

    app.UseSwagger()
        .UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "EasyControll.Api v1");
            c.RoutePrefix = string.Empty;
        });

    app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()).UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();
}
