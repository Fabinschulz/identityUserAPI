using IdentityUser.src.Infra.Settings;

namespace IdentityUser.src.Domain.Common
{
    /// <summary>
    /// Represents a paginated list of data.
    /// </summary>
    /// <typeparam name="T">The type of data in the list.</typeparam>
    public class ListDataPagination<T>
    {
        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int Page { get; set; } = Configuration.DefaultPage;

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        public int TotalPages { get; set; } = Configuration.DefaultPage;

        /// <summary>
        /// Gets or sets the total number of items.
        /// </summary>
        public int TotalItems { get; set; } = Configuration.DefaultTotalItems;

        /// <summary>
        /// Gets or sets the list of data.
        /// </summary>
        public List<T> Data { get; set; } = new List<T>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ListDataPagination{T}"/> class.
        /// </summary>
        /// <param name="data">The list of data.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="size">The total number of pages.</param>
        /// <param name="totalItems">The total number of items.</param>
        public ListDataPagination(List<T> data, int page, int size, int totalItems)
        {
            Data = data ?? new List<T>();
            Page = page;
            TotalPages = size;
            TotalItems = totalItems;
        }
    }
}
