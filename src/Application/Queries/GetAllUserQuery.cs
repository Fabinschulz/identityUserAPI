using IdentityUser.src.Application.Common.Models;
using IdentityUser.src.Application.Queries.DTOs;
using IdentityUser.src.Domain.Entities;

namespace IdentityUser.src.Application.Queries
{
    /// <summary>
    /// Represents a query to get all users with pagination details.
    /// </summary>
    public class GetAllUserQuery
    {
        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the total number of items.
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Gets or sets the list of user data transfer objects.
        /// </summary>
        public List<UserDto> Data { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllUserQuery"/> class.
        /// </summary>
        /// <param name="entity">The entity containing pagination and user data.</param>
        public GetAllUserQuery(ListDataPagination<User> entity)
        {
            Page = entity.Page;
            TotalPages = entity.TotalPages;
            TotalItems = entity.TotalItems;
            Data = entity.Data.Select(user => new UserDto(user)).ToList();
        }
    }
}
