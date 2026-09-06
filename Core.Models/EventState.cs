using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.Models
{
    public class EventState<TPayload>
    {
        public bool Success { get; set; }
        public TPayload? Payload { get; set; }
    }
}
