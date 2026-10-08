using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.Models.AI.Roles;
using ClientManagement.Models.DataTransferObjects;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace ClientManagement.Mcp.Tools
{
    [McpServerToolType]
    public class ClientAndInventorySpecialistTools(IClientService clientService, ILogger<ClientAndInventorySpecialistTools> logger)
    {
        [McpServerTool, DisplayName("create_client"), Description($"Creates a new client if the client doesn't exists or updates an already existing client. [Role: {McpToolAccessRole.ClientAndInventorySpecialist}]")]
        public async Task<bool> CreateClient(ClientDto client)
        {
            logger.LogInformation("client creation process initiated.");
            var created =  await clientService.AddOrUpdate(client);
            var creationStatus = created ? "completed" : "falied";
            logger.LogInformation($"client creation process {creationStatus}.");
            return created;
        }
    }
}
