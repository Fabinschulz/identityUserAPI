﻿using IdentityUser.src.Domain.Settings;

namespace IdentityUser.src.Application.Common.Models
{
    public class ListDataPagination<T>
    {
        public int Page { get; set; } = 0;
        public int TotalPages { get; set; } = 0;
        public int TotalItems { get; set; } = 0;
        public List<T> Data { get; set; } = new List<T>();

        public ListDataPagination(List<T> data, int page, int totalPages, int totalItems)
        {
            Data = data ?? new List<T>();
            Page = page;
            TotalPages = totalPages;
            TotalItems = totalItems;
        }
    }
}
