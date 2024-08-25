using IdentityUser.src.Domain.Entities;
using IdentityUser.src.Domain.Interfaces;
using IdentityUser.src.Infra.Persistence;
using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using IdentityUser.src.Infra.Services.PasswordService;
using IdentityUser.src.Infra.Services.TokenServices;
using IdentityUser.src.Application.Common.Models;
using IdentityUser.src.Application.Common.Exceptions;
using IdentityUser.src.Domain.Enums;

namespace IdentityUser.src.Infra.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {

        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<User> ChangePassword(string email, string password, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
            if (user == null)
            {
                throw new KeyNotFoundException("Usuário não encontrado.");
            }
            user.Password = newPassword;
            await _context.SaveChangesAsync();
            return user;
        }

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

        public async Task<User> Login(string email, string password)
        {
            var user = await GetUserByEmail(email);
            var hashedPassword = PasswordService.HashPassword(password);
            ValidateUserForLogin(user, hashedPassword);

            var token = TokenService.GenerateToken(user);
            return CreateLoggedUser(user, token);
        }

        private static void ValidateUserForLogin(User user, string password)
        {
            if (!PasswordService.VerifyPasswordHash(password, user.Password))
                throw new BadRequestException(new[] { "Senha inválida." });
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

        private async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                throw new KeyNotFoundException("Usuário não encontrado.");
            }

            return user;
        }

        public async Task<User> Register(User user)
        {
            var existingUser = await GetUserByEmail(user.Email);

            if (existingUser != null)
            {
                throw new BadRequestException(new[] { "Usuário já existe com este email." });
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        private IQueryable<User> BuildBaseQuery()
        {
            return _context.Set<User>().AsQueryable();
        }

        public async Task<ListDataPagination<User>> GetAll(int page, int size, string? username, string? email, bool isDeleted, string? orderBy, RoleEnum? role)
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

            var data = await query.Skip((page - 1) * size).Take(size).ToListAsync();

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
