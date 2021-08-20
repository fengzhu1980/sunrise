using System.Collections.Generic;

namespace API.Helpers
{
    public class Pagination<T> where T : class
    {
        public Pagination(int pageNo, int pageSize, int count, IReadOnlyList<T> data)
        {
            PageNo = pageNo;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }

        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
    }
}