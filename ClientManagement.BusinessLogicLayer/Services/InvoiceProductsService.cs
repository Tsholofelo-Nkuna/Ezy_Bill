using AutoMapper;
using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.BusinessLogicLayer.Services.Base;
using ClientManagement.DataAccessLayer;
using ClientManagement.DataAccessLayer.Entities;
using ClientManagement.Presentation.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.BusinessLogicLayer.Services
{
    public class InvoiceProductsService : GenericService<InvoiceProductDto, InvoiceProductsEntity>, IInvoiceProductService
    {
        public InvoiceProductsService(WebDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}
