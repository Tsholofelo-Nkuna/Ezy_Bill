using Core.Presentation.Models.Base;
using Core.Presentation.Models.DataTransferObjects.login;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.Models
{
    public class RegisterViewModel : LoginViewModel
    {
        [Display(Name = "Name"), Required]
        public string CompanyName { get; set; } = string.Empty;
        [Display(Name = "Email"), EmailAddress, Required, DataType(DataType.EmailAddress)]
        public string CompanyEmail
        { 
            get => this.Username;
            set{
                this.Username = value;
            }
        }
        [Display(Name = "Phone"), Required]
        public string CompanyPhone { get; set; } = string.Empty;

        [Display(Name = "Confirm Password"), Required, DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public RegisterViewModel(): base(Enumerable.Empty<UserCredentialsDto>())
        {
        }
        public RegisterViewModel(IEnumerable<UserCredentialsDto> state) : base(state)
        {
        }
    }
}
