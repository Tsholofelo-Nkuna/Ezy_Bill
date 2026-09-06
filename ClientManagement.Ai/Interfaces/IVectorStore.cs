using ClientManagement.BusinessLogicLayer.Interfaces.Base;
using ClientManagement.DataAccessLayer.Entities;
using ClientManagement.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.Ai.Interfaces;

public interface IVectorStore : IGenericService<AppFileDto, AppFileEntity>
{
    public Task<IEnumerable<string>> SearchAsync(string vectorStoreCollectionName, string text);
    public Task<bool> UpSert(string vectorStoreCollectionName, List<AppFileDto> payload);
    public IEnumerable<(string name, string displayName)> GetVectoreStoreNames();
}
