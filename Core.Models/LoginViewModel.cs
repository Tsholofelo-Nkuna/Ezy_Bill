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
    public class LoginViewModel : GenericListViewModel<UserCredentialsDto>
    {
       
        [Display(Name = "Username"), Required, EmailAddress]
        public string Username
        {
            get => this.ViewModelState.FirstOrDefault()?.Username ?? string.Empty;
            set
            {
                if (this.ViewModelState.FirstOrDefault() is UserCredentialsDto validState)
                {
                    validState.Username = value;
                }
                else
                {
                    this.ViewModelState = this.ViewModelState.Append(new UserCredentialsDto { Username = value });
                }
            }
        }
        [Display(Name = "Password"), Required]
        public string Password
        {
            get => this.ViewModelState.FirstOrDefault()?.Password ?? string.Empty;
            set
            {
                if (this.ViewModelState.FirstOrDefault() is UserCredentialsDto validState)
                {
                    validState.Password = value;
                }
                else
                {
                    this.ViewModelState = this.ViewModelState.Append(new UserCredentialsDto { Password = value });
                }
            }
        }
        public LoginViewModel() : this(Enumerable.Empty<UserCredentialsDto>())
        {
        }
        public LoginViewModel(IEnumerable<UserCredentialsDto> state) : base(state)
        {
        }
    }
}
