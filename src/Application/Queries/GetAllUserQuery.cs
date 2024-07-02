using IdentityUser.src.Application.Common.Models;
using IdentityUser.src.Application.Common.Response;
using IdentityUser.src.Domain.Entities;
using IdentityUser.src.Domain.Settings;

namespace IdentityUser.src.Application.Queries
{
    public class GetAllUserQuery : PagedResponse<User>
    {
        public GetAllUserQuery(
           List<User> data,
           int page,
           int size,
           int totalItems,
           string? message = null,
           int code = Configuration.DefaultStatusCode) : base(data, page, size, totalItems, message, code)
        {
        }
    }
}
