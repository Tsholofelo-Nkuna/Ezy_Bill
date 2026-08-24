using ClientManagement.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


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
        public virtual DbSet<ProfileEntity> Profiles { get; set; }
        public virtual DbSet<UserProfileEntity> UserProfiles { get; set; }
        public virtual DbSet<AppFileEntity> AppFiles { get; set; }
    }
}
