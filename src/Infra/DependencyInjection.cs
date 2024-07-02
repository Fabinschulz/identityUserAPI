using FluentValidation;
using IdentityUser.src.Application.Common.Behaviors;
using IdentityUser.src.Domain.Interfaces;
using IdentityUser.src.Infra.Persistence;
using IdentityUser.src.Infra.Repositories;
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
    public static class DependencyInjection
    {
        public static void AddUserContext(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<UserRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
        }

        public static void AddDatabase(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
        }

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

        public class TagDescriptionsDocumentFilter : IDocumentFilter
        {
            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
                swaggerDoc.Tags = new List<OpenApiTag>
                {
                    new OpenApiTag { Name = "USER", Description = "User API" }
                };
            }
        }

        public static void AddAuthPolicy(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("Admin", policy => policy.RequireRole(IdentityData.AdminPolicy));
                opt.AddPolicy("User", policy => policy.RequireRole(IdentityData.UserPolicy));
            });
        }

        public static class IdentityData
        {
            public const string UserPolicy = "User";
            public const string AdminPolicy = "Admin";
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Program));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Add database migration during application startup
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var context = serviceProvider.GetRequiredService<AppDbContext>();
                    var logger = serviceProvider.GetRequiredService<ILogger<AppDbContext>>();

                    context.Database.Migrate();
                    logger.LogInformation("Migração do banco de dados concluída com sucesso.");
                }
                catch (Exception ex)
                {
                    var logger = serviceProvider.GetRequiredService<ILogger<AppDbContext>>();
                    logger.LogError(ex, "Erro durante a migração do banco de dados.");
                    throw new Exception("Erro durante a migração do banco de dados.", ex);
                }
            }

        }
    }
}
