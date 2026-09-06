using AutoMapper;
using ClientManagement.DataAccessLayer.Entities;
using ClientManagement.Models.DataTransferObjects;
using Microsoft.AspNetCore.Identity;



namespace ClientManagement.BusinessLogicLayer
{
    public class AutoMapperConfig : Profile
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
            this.CreateMap<ProfileDto, ProfileEntity>()
                .ReverseMap();
            this.CreateMap<UserProfileDto, UserProfileEntity>()
                .ReverseMap();
            this.CreateMap<AppFileDto, AppFileEntity>()
                .ReverseMap();
            this.CreateMap<IdentityUser, UserDto>()
                .ReverseMap();
        }
    }
}
