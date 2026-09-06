using System;
using System.ComponentModel.DataAnnotations;
using ClientManagement.Models.DataTransferObjects.Base;

namespace ClientManagement.Models.DataTransferObjects
{
    public class InvoicePaymentDto: BaseDto
    {

        [DisplayFormat(DataFormatString ="{0:C}")]
        public double Amount { get; set; }
        public InvoiceDto Invoice { get; set; } = new InvoiceDto();
        public string Comment { get; set; } = string.Empty;
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}")]
        public DateTime? PaymentDate { get; set; }
        public Guid InvoiceId { get; set; }
    }
}
