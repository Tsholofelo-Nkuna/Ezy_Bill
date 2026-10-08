using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.Models.AI.Roles;
using ClientManagement.Models.DataTransferObjects;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace ClientManagement.Mcp.Tools
{
    [McpServerToolType]
    public class ClientAndInventorySpecialistTools(IClientService clientService)
    {
        [McpServerTool, DisplayName("create_client"), Description($"Creates a new client if the client doesn't exists or updates an already existing client. [Role: {McpToolAccessRole.ClientAndInventorySpecialist}]")]
        public async Task<bool> CreateClient(ClientDto client)
        {
            return await clientService.AddOrUpdate(client);
        }
    }
}
