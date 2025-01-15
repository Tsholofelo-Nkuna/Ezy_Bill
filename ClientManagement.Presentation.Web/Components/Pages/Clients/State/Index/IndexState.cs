using ClientManagement.Presentation.Models.DataTransferObjects;
using Core.Presentation.Models;


namespace ClientManagement.Presentation.Web.Components.Pages.Clients.State.Index
{
    public class IndexState
    {
        public ClientDto? NewClientToBeAdded { get; set; }
        public bool NewClientFormIsValid { get; set; }
        public List<InputFieldViewModel<ClientDto>> NewClientDetailsFields { get; set; } = Enumerable.Empty<InputFieldViewModel<ClientDto>>().ToList();
        public List<InputFieldViewModel<ClientDto>> NewClientContactFields { get; set;} = Enumerable.Empty<InputFieldViewModel<ClientDto>>().ToList();
    }
}
