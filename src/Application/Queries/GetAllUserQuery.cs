using IdentityUser.src.Application.Common.Models;
using IdentityUser.src.Application.DTOs;
using IdentityUser.src.Domain.Entities;

namespace IdentityUser.src.Application.Queries
{
    public class GetAllUserQuery
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public List<UserDto> Data { get; set; }

        public GetAllUserQuery(ListDataPagination<User> entity)
        {
            Page = entity.Page;
            TotalPages = entity.TotalPages;
            TotalItems = entity.TotalItems;
            Data = entity.Data.Select(user => new UserDto(user)).ToList();
        }
    }
}
