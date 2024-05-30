using IdentityUser.src.Infra;
using IdentityUser.src.Infra.Services.Extensions;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var builderServices = builder.Services;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builderServices.AddEndpointsApiExplorer();
builderServices.AddSwaggerGen();
builderServices.ConfigureCorsPolicy();
builderServices.AddControllers();
//builder.AddUserContext();
builder.AddDatabase();
builder.AddSwaggerDoc();
builder.AddAuthPolicy();
builder.AddAuthJwt();
builderServices.ConfigureServices();

var app = builder.Build();

var options = new RewriteOptions().AddRedirect("^$", "swagger/index.html");
app.UseRewriter(options);

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Api v1"));
//app.UseErrorHandler();
app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
//app.MapUserEndpoints();

app.Run();
