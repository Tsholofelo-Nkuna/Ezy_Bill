using ClientManagement.DataAccessLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.DataAccessLayer.Entities
{
    public class InvoiceEntity : BaseEntity
    {
        public DateTime? DueDate { get; set; }
        public ClientEntity? Client { get; set; }
        [NotMapped]
        public IEnumerable<Guid> ProductIdentifiers { get; set; } = Enumerable.Empty<Guid>();
      
    }
}
