using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.Models.DataTransferObjects
{
    public class ResponseDto<TData>
    {
        public TData? Data { get; set; } = default(TData);
        public string Message { get; set; } = string.Empty;
        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();
    }
}
