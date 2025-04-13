using ClientManagement.DataAccessLayer.Entities.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.DataAccessLayer.Entities
{
    public class SupportChatEntity : BaseEntity
    {
        public ChatEntity Chat { get; set; }
        public IdentityUser SupportAgent { get; set; }
    }
}
