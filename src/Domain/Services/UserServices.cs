using IdentityUser.src.Domain.Common;
using IdentityUser.src.Domain.Entities;
using IdentityUser.src.Domain.Enums;
using IdentityUser.src.Domain.Interfaces.Repositories;

namespace IdentityUser.src.Domain.Services
{
    /// <summary>
    /// Provides services related to user operations.
    /// </summary>
    public class UserServices
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserServices"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public UserServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Retrieves a paginated list of users based on the specified criteria.
        /// </summary>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="size">The number of users per page.</param>
        /// <param name="username">The username to filter by (optional).</param>
        /// <param name="email">The email to filter by (optional).</param>
        /// <param name="isDeleted">Indicates whether to include deleted users.</param>
        /// <param name="orderBy">The field to order the results by (optional).</param>
        /// <param name="role">The role to filter by (optional).</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a paginated list of users.</returns>
        public async Task<ListDataPagination<User>> GetAllUserAsync(int page, int size, string? username, string? email, bool isDeleted, string? orderBy, RoleEnum? role)
        {
            return await _userRepository.GetAllAsync(page, size, username, email, isDeleted, orderBy, role);
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user associated with the specified identifier.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when a user with the specified identifier is not found.</exception>
        public async Task<User> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"Usuário com o ID '{id}' não encontrado.");
            }
            return user;
        }

        /// <summary>
        /// Asynchronously creates a new user.
        /// </summary>
        /// <param name="user">The user to be created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created user.</returns>
        /// <exception cref="InvalidOperationException">Thrown when a user with the same ID already exists.</exception>
        public async Task<User> CreateUserAsync(User user)
        {
            var existingUser = await _userRepository.GetUserByEmail(user.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException($"Email '{user.Email}' já está em uso.");
            }

            return await _userRepository.Register(user);
        }

        /// <summary>
        /// Updates an existing user asynchronously.
        /// </summary>
        /// <param name="userCommand">The user command containing updated user information.</param>
        /// <returns>The updated user.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when a user with the specified ID is not found.</exception>
        public async Task<User> UpdateUserAsync(User userCommand)
        {
            var user = await _userRepository.GetByIdAsync(userCommand.Id);
            if (user == null)
            {
                throw new KeyNotFoundException($"Usuário com o ID '{userCommand.Id}' não encontrado.");
            }

            user.Update(userCommand.Username, userCommand.Email, userCommand.Role, userCommand.IsDeleted);
            return await _userRepository.UpdateAsync(user);
        }

        /// <summary>
        /// Deletes a user asynchronously by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to be deleted.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when a user with the specified ID is not found.</exception>
        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"Usuário com o ID '{id}' não encontrado.");
            }

            return await _userRepository.DeleteAsync(id);
        }

    }
}