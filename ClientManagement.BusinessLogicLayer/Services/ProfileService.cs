using AutoMapper;
using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.BusinessLogicLayer.Services.Base;
using ClientManagement.DataAccessLayer;
using ClientManagement.DataAccessLayer.Entities;
using Core.Presentation.Models.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


namespace ClientManagement.BusinessLogicLayer.Services
{
    public class ProfileService : GenericService<ProfileDto, ProfileEntity>, IProfileService
    {
        public ProfileService(WebDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager) : base(dbContext, mapper, httpContextAccessor, userManager)
        {
        }
    }
}
