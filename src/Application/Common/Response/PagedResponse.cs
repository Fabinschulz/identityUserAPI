using IdentityUser.src.Application.Common.Models;

namespace IdentityUser.src.Application.Common.Response
{
    public class PagedResponse<T> where T : class
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public List<T> Data { get; set; }

        public PagedResponse(ListDataPagination<T> entity)
        {
            Page = entity.Page;
            TotalPages = entity.TotalPages;
            TotalItems = entity.TotalItems;
            Data = entity.Data;
        }

    }
}
