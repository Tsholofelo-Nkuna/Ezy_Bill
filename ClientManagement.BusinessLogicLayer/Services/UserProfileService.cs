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
    public class UserProfileService : GenericService<UserProfileDto, UserProfileEntity>, IUserProfileService
    {
        public UserProfileService(WebDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager) : base(dbContext, mapper, httpContextAccessor, userManager)
        {
        }

        public async Task<UserProfileDto> CreateProfile(string userId, ProfileDto newProfile)
        {
            if((_userManager.Users.FirstOrDefault(x => x.Id == userId)) is IdentityUser newUser)
            {
                var profile = this._mapper.Map<ProfileEntity>(newProfile);
                if(profile.Id == Guid.Empty)
                {
                    _dbContext.Profiles.Add(profile);
                }
                else
                {
                    _dbContext.Profiles.Update(profile);
                }
              
                var userProfileMap = new UserProfileEntity
                {
                    User = newUser,
                    Profile = profile
                };
                _dbContext.Update(userProfileMap);
                var saveCount = await _dbContext.SaveChangesAsync();
                return _mapper.Map<UserProfileDto>(userProfileMap);
            }
            else
            {
                return new UserProfileDto();
            }
        }
    }
}
