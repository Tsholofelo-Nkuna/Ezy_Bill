using ClientManagement.BusinessLogicLayer.Interfaces.Base;
using ClientManagement.BusinessLogicLayer.Services.Base;
using ClientManagement.DataAccessLayer.Entities;
using ClientManagement.Presentation.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.BusinessLogicLayer.Interfaces
{
    public interface IInvoiceProductService: IGenericService<InvoiceProductDto, InvoiceProductsEntity>
    {

    }
}
