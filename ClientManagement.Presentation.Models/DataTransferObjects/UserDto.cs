using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.Presentation.Models.DataTransferObjects
{
    public class UserDto
    {
        string Id { get; set; } = string.Empty;
        string UserName { get; set; } = string.Empty;
    }
}
