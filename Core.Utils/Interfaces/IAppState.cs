using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils.Interfaces
{
    public interface IAppState
    {
        public Guid ProfileId { get; set; }
        public string Username { get; set; }

    }
}
