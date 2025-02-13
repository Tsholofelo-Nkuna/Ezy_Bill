using AutoMapper;
using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.BusinessLogicLayer.Models;
using ClientManagement.BusinessLogicLayer.Services.Base;
using ClientManagement.DataAccessLayer;
using ClientManagement.DataAccessLayer.Entities;
using Core.Presentation.Models.DataTransferObjects;
using Core.Utils.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.BusinessLogicLayer.Services
{
    public class UserProfileService : GenericService<UserProfileDto, UserProfileEntity>, IUserProfileService
    {
        public UserProfileService(WebDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager,
              IAppStateManager<ApplicationState> appStateManager) : base(dbContext, mapper, httpContextAccessor, userManager, appStateManager)
        {
        }
        public async Task<UserProfileDto> CreateProfile(string userId, ProfileDto newProfile)
        {
            if((_userManager.Users.FirstOrDefault(x => x.Id == userId)) is IdentityUser newUser)
            {
                var profile = this._mapper.Map<ProfileEntity>(newProfile);
                var userProfileMap = _dbContext.UserProfiles.FirstOrDefault(x => x.User.Id == userId && x.Profile.Id == newProfile.Id && !x.Archived);
                //if (profile.Id == Guid.Empty)
                //{
                //    _dbContext.Profiles.Add(profile);
                //}
                //else
                //{
                //    _dbContext.Profiles.Update(profile);
                //}
              
                if(userProfileMap is not null)
                {
                    userProfileMap.Profile = profile;
                }
                else
                {
                     userProfileMap = new UserProfileEntity
                    {
                        User = newUser,
                        Profile = profile
                    };
                }
            
                _dbContext.Update(userProfileMap);
                var saveCount = await _dbContext.SaveChangesAsync();
                return _mapper.Map<UserProfileDto>(userProfileMap);
            }
            else
            {
                return new UserProfileDto();
            }
        }

        public override async  Task<IEnumerable<UserProfileDto>> Get(UserProfileDto filter)
        {
            var query = base.GetQueryable(filter);
            if (filter.User is UserDto userFilter)
            {
                query = query.Where(x => x.User.Id == userFilter.Id);
            }
            var results = await query.Include(x => x.User).Include(x => x.Profile).ToListAsync();
            return  _mapper.Map<List<UserProfileDto>>(results);
        }
    }
}
