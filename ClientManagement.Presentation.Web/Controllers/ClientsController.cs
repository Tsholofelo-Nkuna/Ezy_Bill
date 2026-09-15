

using Microsoft.AspNetCore.Mvc;
using ClientManagement.BusinessLogicLayer.Interfaces;

using ClientManagement.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.BearerToken;
using ClientManagement.Models.DataTransferObjects;
using ClientManagement.Presentation.Web.Controllers.Base;
using Microsoft.Extensions.AI;
using ClientManagement.Ai.Agents;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClientManagement.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ApiBaseController<ClientsController>
    {
        private readonly IClientService _clientService;
        public ClientsController(IClientService clientService, ILogger<ClientsController> logger, BookkeepingAgent bookkeepingAgent): base(logger)
        {
            _clientService = clientService;
        }


        // GET: api/<ClientController>/Get
        [HttpPost("[action]")]
        public async Task<PageResponseDto<ClientDto>> Get(PageRequestDto<ClientDto> pageRequest)
        {
           
            var response = (await this.requestHandler.HandleRequest(async () =>
            {
                return await this.Get(pageRequest, _clientService);
            }, nameof(Get), Task.FromResult<PageResponseDto<ClientDto>>(new() { Items = Enumerable.Empty<ClientDto>() }) ));
            return response;
          
        }

        // GET api/<ClientController>/5
        [HttpGet("Get/{id}")]
        public async Task<ClientDto?> Get(Guid id)
        {
            return await this.requestHandler.HandleRequest(async () => (await _clientService.Get(x => !x.Archived && x.Id == id)).FirstOrDefault(),
                 nameof(Get),
                 Task.FromResult<ClientDto?>(null),
                 id
                );
           
        }

        // GET api/<ClientController>/5
        [HttpGet("GetByArchive/{id}")]
        public async Task<ClientDto?> Get(Guid id,[FromQuery] bool archived)
        {
            return await this.requestHandler.HandleRequest(
                async () => (await _clientService.Get(new PageRequestDto<ClientDto> { Filters = new ClientDto { Id = id, Archived = archived }, GetAllPages = true })).Items.FirstOrDefault(),
                nameof(Get),
                Task.FromResult<ClientDto?>(null),
                id, archived
                );
        }

        // POST api/<ClientController>
        [HttpPost]
        public async Task<bool> Post(ClientDto value)
        {
            return await this.requestHandler.HandleRequest( async () => await this._clientService.AddOrUpdate(value),
                nameof(Post),
                Task.FromResult<bool>(false),
                value
                );
        }

        // GET api/Archive/5
        [HttpGet("[action]/{id}")]
        public async Task<bool> Archive(Guid id)
        {
            return await this.requestHandler.HandleRequest( async() => await this._clientService.Archive(new[] {id}),
                nameof(Archive),
                Task.FromResult<bool>(false),
                id
                );
        }

        // DELETE api/Delete/5
        [HttpDelete("[action]/{id}")]
        public async Task<bool> Delete(Guid id)
        {
            return await this.requestHandler.HandleRequest(async () => await _clientService.Delete(new[] { id }),
                nameof(Delete),
                Task.FromResult<bool>(false), id
                );
        }
    }
}
