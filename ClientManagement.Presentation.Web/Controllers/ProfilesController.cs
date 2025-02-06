using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.Presentation.Models.DataTransferObjects;
using ClientManagement.Presentation.Web.Controllers.Base;
using Core.Presentation.Models.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClientManagement.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class ProfilesController : ApiBaseController<ProfilesController>
    {
        private readonly IProfileService _profileService;
        public ProfilesController(ILogger<ProfilesController> logger, IProfileService profileService) : base(logger)
        {
            _profileService = profileService;
        }

        [HttpPost]
        public async Task<bool> Post(ProfileDto profileDto) {
            return await this.requestHandler
                .HandleRequest( async () =>
                {
                   return await this._profileService.AddOrUpdate(new() { profileDto});
                }, nameof(Post), Task.FromResult(false), profileDto);
        }

        [HttpGet("{id}")]
        public async Task<ProfileDto?> Get(Guid id) 
        {
            return await this.requestHandler.HandleRequest(
                 async () =>
                 {
                     return (await this._profileService.Get(new ProfileDto { Id = id })).FirstOrDefault();
                 }, nameof(Get),
                 Task.FromResult<ProfileDto?>(null),
                 id
                );
        }
    }
}
