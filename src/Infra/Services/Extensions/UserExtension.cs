using IdentityUser.src.Application.Command;
using IdentityUser.src.Application.Common.Models;
using IdentityUser.src.Domain.Entities;
using IdentityUser.src.Domain.Enums;
using IdentityUser.src.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityUser.src.Infra.Services.Extensions
{
    /// <summary>
    /// Provides extension methods for user-related operations.
    /// </summary>
    public static class UserExtension
    {
        /// <summary>
        /// Maps the user endpoints to the web application.
        /// </summary>
        /// <param name="app">The web application.</param>
        public static void MapUserEndpoints(this WebApplication app)
        {
            app.MapPost("/v1/user/register", async (IMediator mediator, ICacheRepository cache, CreateUserCommand command) =>
            {
                var user = await mediator.Send(command);

                await cache.SetValueAsync($"User_GetById_{user.Id}", user, cache.GetCacheOptions());
                await cache.InvalidateCacheAsync("Users_GetAll");
                return Results.Created($"/v1/user/{user.Id}", user);

            }).WithTags("USER").WithSummary("Create a new user");

            app.MapPost("/v1/user/login", async (IMediator mediator, LoginUserCommand command) =>
            {
                var user = await mediator.Send(command);
                return Results.Ok(user);
            }).WithTags("USER").WithSummary("Login a user");


            app.MapPut("/v1/user/{id}", async (IMediator mediator, ICacheRepository cache, Guid id, [FromBody] UpdateUserCommand command) =>
            {
                var updatedCommand = new UpdateUserCommand(id, command.Username, command.Email, command.Role, command.IsDeleted);
                var user = await mediator.Send(updatedCommand);
                await cache.InvalidateCacheAsync($"User_GetById_{id}");
                await cache.InvalidateCacheAsync("Users_GetAll");

                return Results.Ok(user);
            }).WithTags("USER").WithSummary("Update a user");

            app.MapGet("/v1/user/{id}", async (IMediator mediator, ICacheRepository cache, Guid id) =>
            {
                var cacheKey = $"User_GetById_{id}";
                var cachedData = await cache.GetValueAsync<GetUserByIdCommand>(cacheKey);

                if (cachedData != null)
                {
                    return Results.Ok(cachedData);
                }

                var command = new GetUserByIdCommand(id);
                var user = await mediator.Send(command);

                await cache.SetValueAsync(cacheKey, user, cache.GetCacheOptions());
                return Results.Ok(user);

            }).WithTags("USER").WithSummary("Find a user by id");

            app.MapDelete("/v1/user/{id}", async (IMediator mediator, ICacheRepository cache, Guid id) =>
            {
                var command = new DeleteUserCommand(id);
                var user = await mediator.Send(command);

                await cache.InvalidateCacheAsync($"User_GetById_{id}");
                await cache.InvalidateCacheAsync("Users_GetAll");
                return Results.Ok(user);

            }).WithTags("USER").WithSummary("Delete a user").RequireAuthorization("Admin");

            app.MapGet("/v1/user", async (IMediator mediator, ICacheRepository cache, string? username, string? email, bool? isDeleted, string? orderBy, RoleEnum? role, int page = 0, int size = 20) =>
            {
                var cacheKey = $"Users_GetAll";
                var cachedUsers = await cache.GetValueAsync<ListDataPagination<User>>(cacheKey);

                if (cachedUsers != null)
                {
                    return Results.Ok(cachedUsers);
                }

                var getAllUserRequest = new GetAllUserCommand(page, size, username, email, isDeleted ?? false, orderBy, role);
                var users = await mediator.Send(getAllUserRequest);
                await cache.SetValueAsync(cacheKey, users, cache.GetCacheOptions());

                return Results.Ok(users);
            }).WithTags("USER").WithSummary("Get all users");
        }
    }
}