using IdentityUser.src.Domain.Settings;

namespace IdentityUser.src.Application.Common.Response
{
    public class PagedResponse<T> : Response<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = Configuration.DefaultPage;
        public int TotalCount { get; set; }
        public int TotalPageCount => (int)System.Math.Ceiling(decimal.Divide(TotalCount, PageSize));

        public PagedResponse(
            List<T> data,
            int page,
            int size,
            int totalItems,
            string? message = null,
            int code = Configuration.DefaultStatusCode) : base(data, page, size, totalItems, message, code: code)
        {
            CurrentPage = page;
            PageSize = size;
            TotalCount = totalItems;
        }

    }
}
