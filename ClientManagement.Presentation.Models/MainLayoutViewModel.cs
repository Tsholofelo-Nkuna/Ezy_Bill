using Core.Presentation.Models;
using Core.Presentation.Models.Base;
using Core.Presentation.Models.DataTransferObjects;
using Core.Presentation.Models.DataTransferObjects.login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.Presentation.Models
{
    public class MainLayoutViewModel : GenericListViewModel<UserCredentialsDto>
    {
        public MainLayoutViewModel(): this(Enumerable.Empty<UserCredentialsDto>())
        {

        }
        public MainLayoutViewModel(IEnumerable<UserCredentialsDto> state) : base(state)
        {
        }

        public ModalViewModel<UserCredentialsDto> UserProfileModalViewModel { get; set; } = new ModalViewModel<UserCredentialsDto>();
    }
}
