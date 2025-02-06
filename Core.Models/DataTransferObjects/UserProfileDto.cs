using Core.Presentation.Models.DataTransferObjects;
using Core.Presentation.Models.DataTransferObjects.Base;

namespace Core.Presentation.Models.DataTransferObjects
{
    public class UserProfileDto : BaseDto
    {
        public ProfileDto Profile { get; set; } = new ProfileDto();
        public UserDto User { get; set; } = new UserDto();
    }
}
