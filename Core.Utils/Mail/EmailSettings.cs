using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils.Mail
{
    public class EmailSettings
    {
      public string Smtp {  get; set; } = string.Empty;
      public string SendFrom { get; set; }  = string.Empty ;
      public string Password { get; set; } = string.Empty;
      public int Port { get; set; }
    }
}
