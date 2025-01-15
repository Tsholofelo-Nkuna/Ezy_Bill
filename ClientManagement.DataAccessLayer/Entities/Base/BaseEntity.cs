using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.DataAccessLayer.Entities.Base
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public bool Archived { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
