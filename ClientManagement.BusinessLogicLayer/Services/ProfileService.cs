using AutoMapper;
using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.BusinessLogicLayer.Models;
using ClientManagement.BusinessLogicLayer.Services.Base;
using ClientManagement.DataAccessLayer;
using ClientManagement.DataAccessLayer.Entities;
using ClientManagement.Models.DataTransferObjects;
using ClientManagement.Utils.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace ClientManagement.BusinessLogicLayer.Services
{
    public class ProfileService : GenericService<ProfileDto, ProfileEntity>, IProfileService
    {
        public ProfileService(WebDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager,
              IAppStateManager<ApplicationState> appStateManager, ILogger<ProfileService> logger) : base(dbContext, mapper, httpContextAccessor, userManager, appStateManager, logger)
        {
        }
    }
}
