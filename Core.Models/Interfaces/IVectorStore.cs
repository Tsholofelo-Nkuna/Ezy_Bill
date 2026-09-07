
using ClientManagement.Models.DataTransferObjects;

namespace ClientManagemet.Models;

public interface IVectorStore
{
    public Task<IEnumerable<string>> SearchAsync(string vectorStoreCollectionName, string text);
    public Task<bool> UpSert(string vectorStoreCollectionName, List<AppFileDto> payload);
    public IEnumerable<(string name, string displayName)> GetVectoreStoreNames();
}
