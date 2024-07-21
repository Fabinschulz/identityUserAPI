using IdentityUser.src.Application.Common.Models;
using IdentityUser.src.Domain.Entities;
using IdentityUser.src.Domain.Enums;
using System.Security.Claims;

namespace IdentityUser.src.Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<ListDataPagination<User>> GetAll(int Page, int Size, string? Username, string? Email, bool IsDeleted, string? OrderBy, RoleEnum? role);
        Task<User> GetAuthenticatedUser(ClaimsPrincipal user);
        Task<User> Register(User user);
        Task<User> Login(string email, string password);
        Task<User> ChangePassword(string email, string password, string newPassword);        
        Task<User> ForgotPassword(string email);
    }
}
