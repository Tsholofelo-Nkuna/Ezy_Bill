using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.Models.DataTransferObjects.Identity.Interfaces
{
    public interface IErrorResponse
    {
        public Dictionary<string, IEnumerable<string>> Errors { get; set; }
    }
}
