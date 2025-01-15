using ClientManagement.DataAccessLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.DataAccessLayer.Entities
{
    public class InvoicePaymentEntity : BaseEntity
    {

        public double Amount { get; set; }
        public InvoiceEntity Invoice { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
    }
}
