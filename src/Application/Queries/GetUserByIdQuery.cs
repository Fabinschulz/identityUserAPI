using IdentityUser.src.Application.Queries.DTOs;
using IdentityUser.src.Domain.Entities;

namespace IdentityUser.src.Application.Queries
{

    /// <summary>
    /// Represents a query to get a user by their unique identifier.
    /// </summary>
    public sealed class GetUserByIdQuery : UserDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserByIdQuery"/> class.
        /// </summary>
        /// <param name="user">The user entity to map from.</param>
        public GetUserByIdQuery(User user) : base(user)
        {
        }

    }

}
