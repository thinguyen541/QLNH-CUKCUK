using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO
{
    public class PagingHttpResponse
    {
        public object? TotalRecord { get; set; }
        public object? TotalPage { get; set; }
        public object? RecentPage { get; set; }
        public object? PageSize { get; set; }
        public object? Data { get; set; }
    }
}
