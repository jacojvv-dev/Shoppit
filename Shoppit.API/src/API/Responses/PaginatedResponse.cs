using System.Collections.Generic;

namespace API.Responses
{
    public class PaginatedResponse<TResult>
    {
        public List<TResult> Items { get; set; }
        public long TotalItems { get; set; }
        public long TotalPages { get; set; }
        public int Page { get; set; }
        public int? NextPage { get; set; }
        public int? PreviousPage { get; set; }

        private PaginatedResponse()
        {
        }
    }
}