using ClientManagement.Models.DataTransferObjects.Identity.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.Models.DataTransferObjects.Identity
{
    public class ErrorResponseDto : IErrorResponse
    {
        public Dictionary<string, IEnumerable<string>> Errors { get; set; } = new Dictionary<string, IEnumerable<string>>();
    }
}
