using AutoMapper;
using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.BusinessLogicLayer.Interfaces.Base;
using ClientManagement.BusinessLogicLayer.Services.Base;
using ClientManagement.DataAccessLayer;
using ClientManagement.DataAccessLayer.Entities;
using ClientManagement.Presentation.Models.DataTransferObjects;

using Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.BusinessLogicLayer.Services
{
    public class InvoiceService : GenericService<InvoiceDto, InvoiceEntity>, IInvoiceService
    {
        public InvoiceService(WebDbContext dbContext, IMapper mapper,IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager) : base(dbContext, mapper, httpContextAccessor, userManager)
        {
        }

        [Obsolete("Use InvoicePaymentService instead")]
        public  async Task<InvoiceDto?> AddPaymentToInvoice(Guid invoiceId, double amount)
        {
            var invoice = this._entitySet.AsNoTracking().FirstOrDefault(x => x.Id == invoiceId && !x.Archived);
            if(invoice is InvoiceEntity validInvoice)
            {
                this._dbContext.InvoicesPayments.Add(new InvoicePaymentEntity { Amount = amount, Invoice = invoice });
                var invoicePaymentAdded = this._dbContext.SaveChanges() > 0;
                return  (await this.Get(new InvoiceDto { Id = invoiceId })).FirstOrDefault();
            }
            else
            {
                return default;
            }
          
        }

        public  async Task<InvoiceDto?> AddProductsToInvoice(Guid invoiceId, IEnumerable<Guid> productIdentifiers)
        {
            var targetInvoice =  (await this.Get(x => !x.Archived && x.Id == invoiceId)).FirstOrDefault();
            if (targetInvoice is InvoiceDto validInvoiceE)
            {
                var invoiceE = _mapper.Map<InvoiceEntity>(validInvoiceE);
                var addedProducts = this._dbContext
                  .Products
                  .AsNoTracking()
                  .Where(x => productIdentifiers.Contains(x.Id))
                  .AsEnumerable()
                  .Select(x => new InvoiceProductsEntity { Invoice = invoiceE, Product = x, ProductAmount = x.Price });
                this._dbContext.InvoicesProducts.AddRange(addedProducts);
                var productsAdded =  (await this._dbContext.SaveChangesAsync()) > 0;
                return (await this.Get(new InvoiceDto { Id = invoiceId})).FirstOrDefault();
            }
            else
            {
                return default;
            }
          
        }

        public override async Task<bool> AddOrUpdate(List<InvoiceDto> payload)
        {
            var clientIdentifiers = payload
                .Where(x => x.Client != null && x.Client.Id != Guid.Empty).Select(x => x.Client!.Id);
            var clients = (from x in this._dbContext.Clients.AsNoTracking()
                             .Where(y => clientIdentifiers.Contains(y.Id))
                              select x
                             ).ToList();
            payload.ForEach(invoice =>
            {
                if (invoice.Client is not null && invoice.Client.Id != Guid.Empty)
                {
                    invoice.Client = _mapper.Map<ClientDto>(clients.FirstOrDefault(x => x.Id == invoice.Client.Id));
                }
            });
            var toBeUpdated = payload.Where(x => x.Id != Guid.Empty && x.Client is not null && x.Client.Id != Guid.Empty);
            var toBeCreated = payload.Where(x => x.Id == Guid.Empty && x.Client is not null && x.Client.Id != Guid.Empty);
            var created = !toBeCreated.Any();
            var updated = !toBeUpdated.Any();
            var productsGuids = payload
                .SelectMany(x => x.ProductIdentifiers)
                .Distinct()
                .Select(x =>
                {
                    _ = Guid.TryParse(x, out var guidResult);
                    return guidResult;
                }).Where(x => x != Guid.Empty);
            var productsToBeAttachedToInvoice = _dbContext
                .Products.AsNoTracking()
                .Where(p => productsGuids.Contains(p.Id))
                .ToList();
           
            
            if (toBeCreated.Any())
            {
                var updatedInvoices = await this.Update(payload);

                var createdInvoices = (updatedInvoices.Select(x =>
                {
                    var invoiceProducts = productsToBeAttachedToInvoice.Where(p => x.ProductIdentifiers.Contains(p.Id.ToString())).ToList();
                    return new
                    {
                        NewlyCreatedInvoice = _mapper.Map<InvoiceEntity>(x),
                        AttachedProducts = invoiceProducts
                    };
                }).ToList());
               
                if (createdInvoices.Any())
                {
                   
                    var invoiceProducts = createdInvoices.SelectMany(x =>
                    {
                       return x.AttachedProducts.Select(p => new InvoiceProductsEntity {
                            Product = p,
                            Invoice = x.NewlyCreatedInvoice,
                            ProductAmount = p.Price,
                        });
                       
                    });
                    _dbContext.InvoicesProducts.UpdateRange(invoiceProducts);
                    created = (await _dbContext.SaveChangesAsync()) > 0;
                }
               
            }
            return created && updated;
        }

        public async Task<InvoiceDto?> CreateInvoice(DateTime dueDate)
        {
            var addedInvoice = new InvoiceDto { DueDate = dueDate };
            await this.Insert(new List<InvoiceDto> { addedInvoice });
            return (await this.Get(addedInvoice)).FirstOrDefault();
        }

        public override async Task<IEnumerable<InvoiceDto>> Get(InvoiceDto filter)
        {
            var query = this._entitySet.AsNoTracking();
            query = query.Where(x => x.Archived ==  filter.Archived)
                .Include(invoice => invoice.Client)
                .ThenInclude(client=> client.ContactPerson);
           
            var invoiceWithPaymentsQ = from invoiceE in query
                                       join invoiceP in this._dbContext.InvoicesPayments.AsNoTracking()
                                       on invoiceE.Id equals invoiceP.Invoice.Id into invoicePayments
                                       select new
                                       {
                                           Invoice = invoiceE,
                                           PaymentAmount = invoicePayments.Sum(x => x.Amount)
                                      };
            var invoiceAmountQ = (from invoice in query
                                             join invoiceProduct in this._dbContext.InvoicesProducts.AsNoTracking()
                                             .Where(x => !x.Archived).Include(invP => invP.Product)
                                             on invoice.Id equals invoiceProduct.Invoice.Id into invoiceProducts
                                             select new
                                             {
                                                 InvoiceId = invoice.Id,
                                                 InvoiceAmount = invoiceProducts.Sum(x => x.ProductAmount*x.Quantity),
                                                 InvoiceProducts = invoiceProducts
                                             });
            
            var invoicesWithPaymentAndPriceInfoQ = from invoice in invoiceWithPaymentsQ
                                                   join invoiceAmount in invoiceAmountQ
                                                   on invoice.Invoice.Id equals invoiceAmount.InvoiceId
                                                   select new
                                                   {
                                                       invoice.Invoice,
                                                       invoiceAmount.InvoiceAmount,
                                                       invoice.PaymentAmount,
                                                       primaryContact = invoice.Invoice.Client!.ContactPerson.FirstOrDefault(x => x.IsPrimaryContact),
                                                       invoiceAmount.InvoiceProducts
                                                   };
            
            if (filter.DueDate != null && filter.DueDate.Date != DateTime.MinValue.Date)
            {
                invoicesWithPaymentAndPriceInfoQ = invoicesWithPaymentAndPriceInfoQ.Where(x => x.Invoice.DueDate != null && x.Invoice.DueDate.Value.Date <= filter.DueDate.Date);
            }

            if (filter.Id != Guid.Empty)
            {
                invoicesWithPaymentAndPriceInfoQ = invoicesWithPaymentAndPriceInfoQ.Where(x => x.Invoice.Id == filter.Id);
            }

            if (filter.Unpaid)
            {
                invoicesWithPaymentAndPriceInfoQ = invoicesWithPaymentAndPriceInfoQ.Where(x => x.PaymentAmount < x.InvoiceAmount);
            }

            if (!string.IsNullOrWhiteSpace(filter.ClientName))
            {
                invoicesWithPaymentAndPriceInfoQ = invoicesWithPaymentAndPriceInfoQ.Where(inv => inv.Invoice.Client!.CompanyName.Contains(filter.ClientName));
            }

            if (!string.IsNullOrWhiteSpace(filter.PrimaryContact))
            {
                invoicesWithPaymentAndPriceInfoQ = invoicesWithPaymentAndPriceInfoQ.Where(inv => inv.primaryContact.Phone.Contains(filter.PrimaryContact));
            }
            if (!string.IsNullOrWhiteSpace(filter.PrimaryEmail))
            {
                invoicesWithPaymentAndPriceInfoQ = invoicesWithPaymentAndPriceInfoQ.Where(inv => inv.primaryContact.Email.Contains(filter.PrimaryEmail));
            }

            if (!string.IsNullOrWhiteSpace(filter.ClientId) && Guid.TryParse(filter.ClientId, out var clientIdFilter) && clientIdFilter != Guid.Empty)
            {
                invoicesWithPaymentAndPriceInfoQ = invoicesWithPaymentAndPriceInfoQ.Where(x => x.Invoice.Client.Id == clientIdFilter);

            }

            return (await invoicesWithPaymentAndPriceInfoQ.ToListAsync()).Select( x =>
            {
                var invoiceDto = this._mapper.Map<InvoiceDto>(x.Invoice);
              
                invoiceDto.OutstandingAmount = x.InvoiceAmount - x.PaymentAmount;
                invoiceDto.Amount = x.InvoiceAmount;
                invoiceDto.InvoiceProducts = _mapper.Map<IEnumerable<InvoiceProductDto>>(x.InvoiceProducts)
                .Select(x =>
                {
                    x.InvoiceId = invoiceDto.Id;
                    return x;
                });
                invoiceDto.ProductIdentifiers = invoiceDto.InvoiceProducts.Select(x => x.Product.Id.ToString());
               
                return invoiceDto;  
            });
        }
    }
}
