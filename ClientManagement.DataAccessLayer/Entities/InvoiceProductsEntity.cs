using ClientManagement.DataAccessLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.DataAccessLayer.Entities
{
    public class InvoiceProductsEntity : BaseEntity
    {
        public ProductEntity Product { get; set; }
        public double Quantity { get; set; } = 1;
        public InvoiceEntity Invoice { get; set; }
        public double ProductAmount { get; set; }
    }
}
