using Core.Presentation.Models.DataTransferObjects.Base;

namespace Core.Presentation.Models.DataTransferObjects
{
    public class UserProfileDto : BaseDto
    {
        public ProfileDto Profile { get; set; } = new ProfileDto();
        public UserDto User { get; set; } = new UserDto();
        public string ProfileName {
            get => Profile.Name; 
            set => Profile.Name = value;
        }
        public string ProfileEmail 
        {
            get => Profile.Email;
            set => Profile.Email = value; 
        }
        public string ProfilePhone { 
            get => Profile.Phone;
            set => Profile.Phone = value;
        }

        public string Username
        {
            get => User.UserName;
            set => User.UserName = value;
        }

    }
}
