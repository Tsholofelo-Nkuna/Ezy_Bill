using ClientManagement.BusinessLogicLayer.Interfaces.Base;
using ClientManagement.DataAccessLayer.Entities;
using Core.Presentation.Models.DataTransferObjects;

namespace ClientManagement.BusinessLogicLayer.Interfaces
{
    public interface IAppFileService : IGenericService<AppFileDto, AppFileEntity>
    {
        public Task<IEnumerable<string>> SearchAsync(string text);
        public Task<bool> UpSert(string vectorStoreCollectionName, List<AppFileDto> payload);
        public IEnumerable<(string name, string displayName)> GetVectoreStoreNames();
    }
}
