using IdentityUser.src.Application.Common.Models;
using IdentityUser.src.Domain.Settings;
using System.Text.Json.Serialization;

namespace IdentityUser.src.Application.Common.Response
{
    public abstract class Response<TData> : ListDataPagination<TData>
    {
        private int _code = Configuration.DefaultStatusCode;

        [JsonIgnore]
        public bool IsSuccess => _code is >= 200 and < 300;

        public string? Message { get; set; }

        public Response(
            List<TData> data,
            int page,
            int size,
            int totalItems,
            string? message = null,
            int code = Configuration.DefaultStatusCode) : base(data, page, size, totalItems)
        {
            Message = message;
            _code = code;
        }
    }
}
