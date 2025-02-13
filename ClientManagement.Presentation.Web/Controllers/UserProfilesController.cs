using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.Presentation.Web.Controllers.Base;
using Core.Presentation.Models.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;


namespace ClientManagement.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfilesController : ApiBaseController<UserProfilesController>
    {
        private readonly IUserProfileService _userProfileService;
        public UserProfilesController(ILogger<UserProfilesController> logger, IUserProfileService userProfileService) : base(logger)
        {
            _userProfileService = userProfileService;
        }

        [HttpPost("{userId}")]
        public async Task<ResponseDto<UserProfileDto>> Post(string userId, ProfileDto profile) 
        {
             var result = await this.requestHandler.HandleRequest(async () =>
             {
                 return await _userProfileService.CreateProfile(userId, profile);
             }, nameof(Post), Task.FromResult(new UserProfileDto()), userId, profile);
            return new()
            {
                Data = result,
                Message = result?.Profile?.Id is not null && result.Profile.Id != Guid.Empty ? "User profile created successfully" : "Failed to create user profile"
            };
        }

        [HttpGet("{userId}")]
        public async Task<UserProfileDto?> Get(string userId)
        { 
            return await this.requestHandler.HandleRequest(async () =>
            {
                return (await _userProfileService.Get(new UserProfileDto { User = new() { Id = userId } })).FirstOrDefault();
            }, nameof(Get), Task.FromResult(new UserProfileDto()), userId);
        }
    }
}
