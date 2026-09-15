using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.Models.DataTransferObjects
{
    public class PageRequestDto<TFilter> where TFilter : new()
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; } = 1;
        public TFilter Filters { get; set; } = new TFilter();
        public bool GetAllPages { get; set; }

    }
}
