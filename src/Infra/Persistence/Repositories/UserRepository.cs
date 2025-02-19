using IdentityUser.src.Application.Common.Exceptions;
using IdentityUser.src.Domain.Common;
using IdentityUser.src.Domain.Entities;
using IdentityUser.src.Domain.Enums;
using IdentityUser.src.Domain.Interfaces.Repositories;
using IdentityUser.src.Infra.Persistence.Database;
using IdentityUser.src.Infra.Services.PasswordService;
using IdentityUser.src.Infra.Services.TokenServices;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;

namespace IdentityUser.src.Infra.Persistence.Repositories
{
    /// <summary>
    /// Repository class for managing user data.
    /// </summary>
    public class UserRepository : BaseRepository<User>, IUserRepository
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Changes the password for the user with the specified email and current password.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="currentPassword">The current password of the user.</param>
        /// <param name="newPassword">The new password to set for the user.</param>
        /// <returns>The user with the updated password.</returns>
        public async Task<User> ChangePassword(Guid userId, string currentPassword, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new KeyNotFoundException("Usuário não encontrado.");
            }

            if (currentPassword != user.Password)
            {
                string errorMessage = "A senha informada não confere com a senha cadastrada.";
                throw new BadRequestException(errorMessage);
            }

            user.Password = newPassword;
            return user;
        }

        /// <summary>
        /// Resets the password for the user with the specified email.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <returns>The user with the reset password.</returns>
        public async Task<User> ForgotPassword(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                throw new KeyNotFoundException("Usuário não encontrado.");
            }
            user.Password = "123456";
            await _context.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// Gets the authenticated user based on the provided claims principal.
        /// </summary>
        /// <param name="user">The claims principal containing the user's claims.</param>
        /// <returns>The authenticated user.</returns>
        public async Task<User> GetAuthenticatedUser(ClaimsPrincipal user)
        {
            var email = user.FindFirst(ClaimTypes.Email)?.Value;
            var password = user.FindFirst(ClaimTypes.Hash)?.Value;
            var userEntity = await _context.Users.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
            if (userEntity == null)
            {
                throw new KeyNotFoundException("Usuário não encontrado.");
            }
            return userEntity;
        }

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user if found; otherwise, null.</returns>
        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Logs in a user with the specified email and password.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>The logged-in user with a generated token.</returns>
        public async Task<User> Login(string email, string password)
        {
            var user = await GetUserByEmail(email);

            if (user == null)
            {
                throw new BadRequestException(new[] { "Email não cadastrado." });
            }

            var hashedPassword = PasswordService.HashPassword(password);
            ValidateUserForLogin(user, hashedPassword);

            var token = TokenService.GenerateToken(user);
            return CreateLoggedUser(user, token);
        }

        private static void ValidateUserForLogin(User user, string password)
        {
            if (!PasswordService.VerifyPasswordHash(password, user.Password))
                throw new BadRequestException(new[] { "Senha incorreta. Verifique suas credenciais e tente novamente." });
        }

        private User CreateLoggedUser(User user, string token)
        {
            var loggedUser = new User(user.Email, user.Password)
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                Token = token
            };
            return loggedUser;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="user">The user to register.</param>_
        public async Task<User> Register(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
            return user;
        }

        private IQueryable<User> BuildBaseQuery()
        {
            return _context.Set<User>().AsQueryable();
        }

        /// <summary>
        /// Gets a paginated list of users based on the specified filters and sorting options.
        /// </summary>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="size">The number of items per page.</param>
        /// <param name="username">The username to filter by.</param>
        /// <param name="email">The email to filter by.</param>
        /// <param name="isDeleted">The flag to filter by deleted status.</param>
        /// <param name="orderBy">The field to order by.</param>
        /// <param name="role">The role to filter by.</param>
        /// <returns>The paginated list of users.</returns>
        public async Task<ListDataPagination<User>> GetAllAsync(int page, int size, string? username, string? email, bool isDeleted, string? orderBy, RoleEnum? role)
        {
            var query = BuildBaseQuery();

            ApplyUsernameFilter(ref query, username);
            ApplyEmailFilter(ref query, email);
            ApplyIsDeletedFilter(ref query, isDeleted);
            ApplyRoleFilter(ref query, role);

            if (!string.IsNullOrEmpty(orderBy))
            {
                query = ApplyOrderBy(query, orderBy);
            }

            var totalItems = await query.CountAsync();
            var data = await query.Skip(page * size).Take(size).ToListAsync();

            return new ListDataPagination<User>(data, page, size, totalItems);
        }

        private static void ApplyUsernameFilter(ref IQueryable<User> query, string? username)
        {
            username = username?.ToLower().Trim();
            ApplyFilterIfNotEmpty(username, x => EF.Property<string>(x, "Username").ToLower().Contains(username!), ref query);
        }

        private static void ApplyEmailFilter(ref IQueryable<User> query, string? email)
        {
            ApplyFilterIfNotEmpty(email, x => EF.Property<string>(x, "Email") != null && EF.Property<string>(x, "Email").Contains(email!), ref query);
        }

        private static void ApplyIsDeletedFilter(ref IQueryable<User> query, bool isDeleted)
        {
            ApplyFilterIfTrue(isDeleted, x => EF.Property<bool>(x, "IsDeleted") == isDeleted, x => EF.Property<bool?>(x, "IsDeleted") == false || EF.Property<bool?>(x, "IsDeleted") == null, ref query);
        }

        private static void ApplyRoleFilter(ref IQueryable<User> query, RoleEnum? role)
        {
            if (role.HasValue)
            {
                var roleValue = role.Value;
                query = query.Where(x => x.Role == roleValue);
            }
        }

        private static void ApplyFilterIfNotEmpty(string? value, Expression<Func<User, bool>> filter, ref IQueryable<User> query)
        {
            if (!string.IsNullOrEmpty(value))
            {
                query = query.Where(filter);
            }
        }

        private static void ApplyFilterIfTrue(bool condition, Expression<Func<User, bool>> filterTrue, Expression<Func<User, bool>> filterFalse, ref IQueryable<User> query)
        {
            query = query.Where(condition ? filterTrue : filterFalse);
        }

        private static IQueryable<User> ApplyOrderBy(IQueryable<User> query, string orderBy)
        {
            switch (orderBy)
            {
                case "createdAt_ASC":
                    return query.OrderBy(x => x.CreatedAt);
                case "createdAt_DESC":
                    return query.OrderByDescending(x => x.CreatedAt);
                case "username_ASC":
                    return query.OrderBy(x => x.Username);
                case "username_DESC":
                    return query.OrderByDescending(x => x.Username);
                case "email_ASC":
                    return query.OrderBy(x => x.Email);
                case "email_DESC":
                    return query.OrderByDescending(x => x.Email);
                case "role_ASC":
                    return query.OrderBy(x => x.Role);
                case "role_DESC":
                    return query.OrderByDescending(x => x.Role);
                default:
                    return query.OrderByDescending(x => x.CreatedAt);
            }
        }
    }
}
