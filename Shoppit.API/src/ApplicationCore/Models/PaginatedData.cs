using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Models
{
    public class PaginatedData<T>
    {
        public List<T> Items { get; private set; }
        public long TotalItems { get; private set; }
        public long TotalPages { get; private set; }
        public int Page { get; private set; }
        public int? NextPage { get; private set; }
        public int? PreviousPage { get; private set; }

        public PaginatedData(List<T> items, long total, int page, int perPage)
        {
            Items = items;
            TotalItems = total;
            TotalPages = CalculateTotalPages(total, perPage);
            Page = page;
            NextPage = CalculateNextPage();
            PreviousPage = CalculatePreviousPage();
        }

        public PaginatedData(IReadOnlyCollection<T> items, long total, int page, int perPage)
        {
            Items = items.ToList();
            TotalItems = total;
            TotalPages = CalculateTotalPages(total, perPage);
            Page = page;
            NextPage = CalculateNextPage();
            PreviousPage = CalculatePreviousPage();
        }

        private static int CalculateTotalPages(long total, int perPage)
            => Math.Clamp((int) Math.Ceiling(total / (decimal) perPage), 1, int.MaxValue);

        private int? CalculatePreviousPage()
            => Page > 1 ? Page - 1 : (int?) null;

        private int? CalculateNextPage()
            => TotalPages > Page ? Page + 1 : (int?) null;
    }
}