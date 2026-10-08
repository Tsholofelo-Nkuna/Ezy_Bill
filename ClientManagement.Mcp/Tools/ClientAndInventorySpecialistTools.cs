using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.Models.DataTransferObjects;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace ClientManagement.Mcp.Tools
{
    [McpServerToolType]
    public class ClientAndInventorySpecialistTools(IClientService clientService)
    {
        [McpServerTool, DisplayName("create_client")]
        public async Task<bool> CreateClient(ClientDto client)
        {
            return await clientService.AddOrUpdate(client);
        }
    }
}
