using IdentityUser.src.Domain.Common;
using IdentityUser.src.Domain.Entities;
using IdentityUser.src.Domain.Enums;
using System.Security.Claims;

namespace IdentityUser.src.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Interface for user repository operations.
    /// </summary>
    public interface IUserRepository : IBaseRepository<User>
    {

        /// <summary>
        /// Retrieves a paginated list of users based on the provided filters.
        /// </summary>
        /// <param name="Page">The page number to retrieve.</param>
        /// <param name="Size">The number of items per page.</param>
        /// <param name="Username">The username to filter by.</param>
        /// <param name="Email">The email to filter by.</param>
        /// <param name="IsDeleted">The deleted status to filter by.</param>
        /// <param name="OrderBy">The field to order by.</param>
        /// <param name="role">The role to filter by.</param>
        /// <returns>A paginated list of users.</returns>
        Task<ListDataPagination<User>> GetAll(int Page, int Size, string? Username, string? Email, bool IsDeleted, string? OrderBy, RoleEnum? role);

        /// <summary>
        /// Retrieves the authenticated user based on the provided claims principal.
        /// </summary>
        /// <param name="user">The claims principal representing the authenticated user.</param>
        /// <returns>The authenticated user.</returns>
        Task<User> GetAuthenticatedUser(ClaimsPrincipal user);

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="user">The user to register.</param>
        /// <returns>The registered user.</returns>
        Task<User> Register(User user);

        /// <summary>
        /// Logs in a user with the provided email and password.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>The logged-in user.</returns>
        Task<User> Login(string email, string password);

        /// <summary>
        /// Changes the password for a user.
        /// </summary>
        /// <param name="userId">The email of the user.</param>
        /// <param name="currentPassword">The current password of the user.</param>
        /// <param name="newPassword">The new password for the user.</param>
        /// <returns>The user with the updated password.</returns>
        Task<User> ChangePassword(Guid userId, string currentPassword, string newPassword);

        /// <summary>
        /// Initiates the forgot password process for a user.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <returns>The user for whom the password reset was initiated.</returns>
        Task<User> ForgotPassword(string email);


    }
}
