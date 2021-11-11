using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.RequestParameters
{
    public class PagedList<T> : List<T>
    {
        public PageData PageData { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            PageData = new PageData
            {
                PageCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
            };

            AddRange(items);
        }

        public static PagedList<T> ToPageList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }


    }

    public class PageData
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
    }
}
