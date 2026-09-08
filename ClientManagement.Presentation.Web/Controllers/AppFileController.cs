
using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.Presentation.Web.Controllers.Base;
using ClientManagement.Models.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using ClientManagement.BusinessLogicLayer.Helpers.Interface;

namespace ClientManagement.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppFileController : ApiBaseController<AppFileController>
    {
        private readonly ILogger<AppFileController> logger;
        private readonly IAppFileService _appFileService;
        private readonly IVectorStore _vectorStore;

        public AppFileController(ILogger<AppFileController> logger, IAppFileService appFileService, IVectorStore vectorStore) : base(logger)
        {
            this.logger = logger;
            _appFileService = appFileService;
            this._vectorStore = vectorStore;
            // _appVectorStore = appVectorStore;
        }

        // POST api/<AppFileController>/{vectorStoreName}
        [HttpPost("{vectorStoreName}")]
        public async Task<bool> Post([FromBody] List<AppFileDto> value, string vectorStoreName)
        {
            var serviceReponse = await this._appFileService.AddOrUpdate(value);
            if (serviceReponse)
            {
                return await this._vectorStore.UpSert(vectorStoreName, value, value.FirstOrDefault()?.AgentName ?? string.Empty);
            }
            else
            {
                return false;
            }
           
        }

        [HttpPost("[action]")]
        public Task<PageResponseDto<AppFileDto>> Get(PageRequestDto<AppFileDto> pageRequest)
        {
            return base.Get(pageRequest, _appFileService);
        }

        [HttpGet("[action]")]
        public IEnumerable<Dictionary<string, string>> VectoreStoreNames()
        {
            return this._vectorStore.GetVectoreStoreNames().Select(x =>
            {
                return new Dictionary<string, string> { ["Name"] = x.name, ["DisplayName"] = x.displayName, ["AgentName"] = x.agentName };
            });
        }

    }
}
