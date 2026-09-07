using ClientManagement.Models.DataTransferObjects;

namespace ClientManagement.BusinessLogicLayer.Interfaces;

public interface IVectorStore
{
    public Task<IEnumerable<string>> SearchAsync(string vectorStoreCollectionName, string text);
    public Task<bool> UpSert(string vectorStoreCollectionName, List<AppFileDto> payload);
    public IEnumerable<(string name, string displayName)> GetVectoreStoreNames();
}
