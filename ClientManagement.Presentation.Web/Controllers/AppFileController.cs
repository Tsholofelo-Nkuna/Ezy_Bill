
using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.Presentation.Web.Controllers.Base;
using ClientManagement.Models.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using ClientManagemet.Models;

namespace ClientManagement.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppFileController : ApiBaseController<AppFileController>
    {
        private readonly IVectorStore _appVectorStore;
        private readonly IAppFileService _appFileService;

        public AppFileController(ILogger<AppFileController> logger, IVectorStore appVectorStore, IAppFileService appFileService) : base(logger)
        {
            _appFileService = appFileService;
            _appVectorStore = appVectorStore;
        }

        // POST api/<AppFileController>/{vectorStoreName}
        [HttpPost("{vectorStoreName}")]
        public async Task<bool> Post([FromBody] List<AppFileDto> value, string vectorStoreName)
        {
            var serviceReponse = await this._appFileService.AddOrUpdate(value);
            if (serviceReponse)
            {
                return await this._appVectorStore.UpSert(vectorStoreName, value);
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
            return this._appVectorStore.GetVectoreStoreNames().Select(x =>
            {
                return new Dictionary<string, string> { ["Name"] = x.name, ["DisplayName"] = x.displayName };
            });
        }

    }
}
