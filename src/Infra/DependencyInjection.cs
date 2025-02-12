using FluentValidation;
using IdentityUser.src.Application.Common.Behaviors;
using IdentityUser.src.Domain.Enums;
using IdentityUser.src.Domain.Interfaces;
using IdentityUser.src.Infra.Persistence;
using IdentityUser.src.Infra.Repositories;
using IdentityUser.src.Infra.Services.Extensions;
using IdentityUser.src.Infra.Settings;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;

namespace IdentityUser.src.Infra
{
    /// <summary>
    /// Provides extension methods for adding services to the dependency injection container.
    /// </summary>
    public static class DependencyInjection
    {

        /// <summary>
        /// Adds the user context to the service collection.
        /// </summary>
        /// <param name="builder">The web application builder.</param>
        /// <returns>The web application builder.</returns>
        public static void AddUserContext(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<UserRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
        }

        /// <summary>
        /// Adds the database context to the service collection.
        /// </summary>
        /// <param name="builder">The web application builder.</param>
        public static void AddDatabase(this WebApplicationBuilder builder)
        {
            string connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection")!;
            Console.WriteLine("Initializing Database for API: " + connectionString.Substring(0, 49));

            try
            {
                builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error connecting to database: " + e.Message);
                throw new Exception("Error on postgresql: " + connectionString.Substring(0, 49));
            }

        }

        /// <summary>
        /// Adds JWT authentication to the service collection.
        /// </summary>
        /// <param name="builder">The web application builder.</param>
        public static void AddAuthJwt(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                var key = Encoding.ASCII.GetBytes("MySecretKey");
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        /// <summary>
        /// Adds Swagger documentation to the service collection.
        /// </summary>
        /// <param name="builder">The web application builder.</param>
        public static void AddSwaggerDoc(this WebApplicationBuilder builder)
        {

            builder.Services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new()
                {
                    Title = "User API",
                    Version = "v1",
                    Description = "Uma API para autenticação de usuários",
                    Contact = new()
                    {
                        Name = "Fabio Lima",
                        Email = "fabio.lima19997@gmail.com"
                    }

                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);
                config.DocumentFilter<TagDescriptionsDocumentFilter>();
            });
        }

        /// <summary>
        /// Adds Swagger documentation to the service collection.
        /// </summary>
        public class TagDescriptionsDocumentFilter : IDocumentFilter
        {
            /// <summary>
            /// Applies the filter to the OpenApiDocument.
            /// </summary>
            /// <param name="swaggerDoc"></param>
            /// <param name="context"></param>
            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
                swaggerDoc.Tags = new List<OpenApiTag>
                {
                    new OpenApiTag { Name = "USER", Description = "User API" }
                };
            }
        }

        /// <summary>
        /// Adds authorization policies to the service collection.
        /// </summary>
        /// <param name="builder">The web application builder.</param>
        public static void AddAuthPolicy(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("Admin", policy => policy.RequireRole(IdentityData.AdminPolicy));
                opt.AddPolicy("User", policy => policy.RequireRole(IdentityData.UserPolicy));
            });
        }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Program));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.ConfigureCorsPolicy();

            services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new RoleEnumConverter());
            });

            services.AddHostedService<MigrationHostedService>();

        }
    }
}
