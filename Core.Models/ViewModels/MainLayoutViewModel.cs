using Core.Presentation.Models.Base;
using Core.Presentation.Models.DataTransferObjects.login;


namespace Core.Presentation.Models.ViewModels
{
    public class MainLayoutViewModel : GenericListViewModel<UserCredentialsDto>
    {
        public bool ShowSideMenuItems { get; set; } = false;
        public float InnerWidth { get; set; }
        public MainLayoutViewModel(): this(Enumerable.Empty<UserCredentialsDto>())
        {

        }
        public MainLayoutViewModel(IEnumerable<UserCredentialsDto> state) : base(state)
        {
        }

        public ModalViewModel<UserCredentialsDto> UserProfileModalViewModel { get; set; } = new ModalViewModel<UserCredentialsDto>();
    }
}
