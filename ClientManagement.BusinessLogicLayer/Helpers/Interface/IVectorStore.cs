using ClientManagement.Models.DataTransferObjects;

namespace ClientManagement.BusinessLogicLayer.Helpers.Interface;

public interface IVectorStore
{
    public Task<IEnumerable<string>> SearchAsync(string vectorStoreCollectionName, string text, string agentName);
    public Task<bool> UpSert(string vectorStoreCollectionName, List<AppFileDto> payload, string agentName);
    public IEnumerable<(string name, string displayName, string agentName)> GetVectoreStoreNames();
  
}
