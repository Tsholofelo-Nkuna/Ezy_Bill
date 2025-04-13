using ClientManagement.DataAccessLayer.Entities.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.DataAccessLayer.Entities
{
    public class ChatEntity : BaseEntity
    {
        public IdentityUser Sender { get; set; }
        /// <summary>
        /// Base64 string
        /// </summary>
        public string? Attachment {  get; set; }
        public string Message { get; set; }


    }
}
