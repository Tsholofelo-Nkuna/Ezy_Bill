using ClientManagement.DataAccessLayer.Entities.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.DataAccessLayer.Entities
{
    public class UserProfileEntity : BaseEntity
    {
        public ProfileEntity Profile { get; set; }
        public IdentityUser User { get; set; }
    }
}
