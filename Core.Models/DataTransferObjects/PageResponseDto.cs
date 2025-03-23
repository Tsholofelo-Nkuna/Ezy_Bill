using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.Models.DataTransferObjects
{
    public class PageResponseDto<TData>
    {
        public int TotalRecords { get; set; }
        public IEnumerable<TData> Items { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

    }
}
