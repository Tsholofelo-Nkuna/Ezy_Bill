

using Microsoft.AspNetCore.Mvc;
using ClientManagement.BusinessLogicLayer.Interfaces;

using Core.Utils;
using ClientManagement.Presentation.Models.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.BearerToken;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClientManagement.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly ILogger<ClientsController> _logger;
        private readonly ControllerRequestHandler<ClientsController> _requestHandler;
        public ClientsController(IClientService clientService, ILogger<ClientsController> logger)
        {
            _clientService = clientService;
            _logger = logger;
            _requestHandler = new ControllerRequestHandler<ClientsController>(_logger);
        }


        // GET: api/<ClientController>/Get
        [HttpPost("[action]")]
        public async Task<IEnumerable<ClientDto>> Get(ClientDto filter)
        {
            var response = (await _requestHandler.HandleRequest(async () =>
            {
                return await this._clientService.Get(filter);
            }, nameof(Get), Task.FromResult(Enumerable.Empty<ClientDto>()) ));
            return response;
          
        }

        // GET api/<ClientController>/5
        [HttpGet("Get/{id}")]
        public async Task<ClientDto?> Get(Guid id)
        {
            return await this._requestHandler.HandleRequest(async () => (await _clientService.Get(x => !x.Archived && x.Id == id)).FirstOrDefault(),
                 nameof(Get),
                 Task.FromResult<ClientDto?>(null),
                 id
                );
           
        }

        // GET api/<ClientController>/5
        [HttpGet("GetByArchive/{id}")]
        public async Task<ClientDto?> Get(Guid id,[FromQuery] bool archived)
        {
            return await this._requestHandler.HandleRequest(
                async () => (await _clientService.Get(new ClientDto { Id = id, Archived = archived })).FirstOrDefault(),
                nameof(Get),
                Task.FromResult<ClientDto?>(null),
                id, archived
                );
        }

        // POST api/<ClientController>
        [HttpPost]
        public async Task<bool> Post(ClientDto value)
        {
            return await this._requestHandler.HandleRequest( async () => await this._clientService.AddOrUpdate(value),
                nameof(Post),
                Task.FromResult<bool>(false),
                value
                );
        }

        // GET api/Archive/5
        [HttpGet("[action]/{id}")]
        public async Task<bool> Archive(Guid id)
        {
            return await this._requestHandler.HandleRequest( async() => await this._clientService.Archive(new[] {id}),
                nameof(Archive),
                Task.FromResult<bool>(false),
                id
                );
        }

        // DELETE api/Delete/5
        [HttpDelete("[action]/{id}")]
        public async Task<bool> Delete(Guid id)
        {
            return await this._requestHandler.HandleRequest(async () => await _clientService.Delete(new[] { id }),
                nameof(Delete),
                Task.FromResult<bool>(false), id
                );
        }
    }
}
