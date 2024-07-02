using IdentityUser.src.Application.Requests;
using MediatR;

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

        }

    }
}
