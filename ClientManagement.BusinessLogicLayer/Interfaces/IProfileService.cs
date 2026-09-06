using ClientManagement.BusinessLogicLayer.Interfaces.Base;
using ClientManagement.DataAccessLayer.Entities;
using ClientManagement.Models.DataTransferObjects;


namespace ClientManagement.BusinessLogicLayer.Interfaces
{
    public interface IProfileService : IGenericService<ProfileDto, ProfileEntity>
    {
    }
}
