using ClientManagement.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.DataAccessLayer
{
    public class WebDbContext : IdentityDbContext
    {
        public WebDbContext(DbContextOptions<WebDbContext> options) : base(options) { }

        public virtual DbSet<ClientEntity> Clients { get; set; }

        public virtual DbSet<ContactPersonEntity> Contacts { get; set; }
        public virtual DbSet<ProductEntity> Products { get; set; }
        public virtual DbSet<InvoiceEntity> Invoices { get; set; }
        public virtual DbSet<InvoiceProductsEntity> InvoicesProducts { get; set; }
        public virtual DbSet<InvoicePaymentEntity> InvoicesPayments { get; set; }
    }
}
