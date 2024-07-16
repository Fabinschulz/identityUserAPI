using IdentityUser.src.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Builder;

namespace IdentityUser.src.Infra.Services.Extensions
{
    public static class UserExtension
    {
        public static void MapUserEndpoints(this WebApplication app)
        {

            app.MapPost("/v1/user", async (IMediator mediator, CreateUserCommand command) =>
            {
                var user = await mediator.Send(command);
                return Results.Created($"/v1/user/{user.Id}", user);

            }).WithTags("USER").WithSummary("Create a new user");


            app.MapPut("/v1/user/{id}", async (IMediator mediator, Guid id, UpdateUserCommand command) =>
            {
                var updatedCommand = new UpdateUserCommand(id, command.Username, command.Email, command.Role, command.IsDeleted);
                var user = await mediator.Send(updatedCommand);
                return Results.Ok(user);

            }).WithTags("USER").WithSummary("Update a user");

            app.MapGet("/v1/user/{id}", async (IMediator mediator, Guid id) =>
            {
                var command = new GetUserByIdCommand(id);
                var user = await mediator.Send(command);
                return Results.Ok(user);

            }).WithTags("USER").WithSummary("Find a user by id");

        }

    }
}
