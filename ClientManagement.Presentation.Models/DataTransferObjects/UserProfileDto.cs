using Core.Presentation.Models.DataTransferObjects.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.Presentation.Models.DataTransferObjects
{
    public class UserProfileDto : BaseDto
    {
        public ProfileDto Profile { get; set; } = new ProfileDto();
        public UserDto User { get; set; } = new UserDto();
    }
}
