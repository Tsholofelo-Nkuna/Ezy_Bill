using ClientManagement.BusinessLogicLayer.Interfaces.Base;
using ClientManagement.DataAccessLayer.Entities;
using Core.Presentation.Models.DataTransferObjects;

namespace ClientManagement.BusinessLogicLayer.Interfaces
{
    public interface IAppFileService : IGenericService<AppFileDto, AppFileEntity>
    {
    }
}
