using IdentityUser.src.Infra;
using IdentityUser.src.Presentation.Api;
using IdentityUser.src.Presentation.Middleware;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);
var builderServices = builder.Services;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.AddRepositories();
builder.AddDatabase();
builder.AddSwaggerDoc();
builder.AddAuthPolicy();
builder.AddAuthJwt();
builderServices.ConfigureServices();
builderServices.Redis(builder.Configuration);

var app = builder.Build();

var options = new RewriteOptions().AddRedirect("^$", "swagger/index.html");
app.UseRewriter(options);

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Api v1"));
app.MapSwagger();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.MapUserEndpoints();
app.Run();
