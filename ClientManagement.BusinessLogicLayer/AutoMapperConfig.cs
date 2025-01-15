using AutoMapper;
using ClientManagement.DataAccessLayer.Entities;
using ClientManagement.Presentation.Models.DataTransferObjects;



namespace ClientManagement.BusinessLogicLayer
{
    internal class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() {
            this.
                CreateMap<ClientDto, ClientEntity>()
                .ReverseMap();
            this
                .CreateMap<ContactPersonDto, ContactPersonEntity>()
                .ReverseMap();
            this.CreateMap<ProductDto, ProductEntity>()
                .ReverseMap();
            this.CreateMap<InvoiceDto, InvoiceEntity>()
                .ReverseMap();
            this.CreateMap<InvoiceProductDto, InvoiceProductsEntity>()
                .ReverseMap();
            this.CreateMap<InvoicePaymentDto, InvoicePaymentEntity>()
                .ReverseMap();
        }
    }
}
