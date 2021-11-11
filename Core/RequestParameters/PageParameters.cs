using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.RequestParameters
{
    public class PageParameters
    {
        const int MaxPageSize = 1000;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 100;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

    }
}
