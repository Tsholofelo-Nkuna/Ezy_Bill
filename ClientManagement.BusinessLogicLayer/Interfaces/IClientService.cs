using ClientManagement.BusinessLogicLayer.Interfaces.Base;
using ClientManagement.DataAccessLayer.Entities;
using Core.Presentation.Models.DataTransferObjects;


namespace ClientManagement.BusinessLogicLayer.Interfaces
{
    public interface IClientService : IGenericService<ClientDto, ClientEntity>
    {
       

         Task<bool> AddOrUpdate(ClientDto client);
         Task<bool> Archive(IEnumerable<Guid> identifiers);
    }
}
