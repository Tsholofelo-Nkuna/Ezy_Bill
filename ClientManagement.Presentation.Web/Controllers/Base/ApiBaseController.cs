using ClientManagement.BusinessLogicLayer.Interfaces.Base;
using ClientManagement.Models.DataTransferObjects;
using ClientManagement.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ClientManagement.Presentation.Web.Controllers.Base
{
   
    [ApiController]
    public class ApiBaseController<TCategoryName> : ControllerBase
    {
        protected readonly ILogger<TCategoryName> logger;
        protected readonly ControllerRequestHandler<TCategoryName> requestHandler;
        public ApiBaseController(ILogger<TCategoryName> logger)
        {
            this.logger = logger;
            this.requestHandler = new ControllerRequestHandler<TCategoryName>(this.logger);
        }

        protected virtual async Task<PageResponseDto<TDto>> Get<TDto, TEntity>(PageRequestDto<TDto> pRequest, IGenericService<TDto, TEntity> service) where TDto: new(){ 
           var response =   await service.Get(pRequest);

            return new PageResponseDto<TDto> { 
              Items = response.Items,
              PageIndex = pRequest.PageIndex,
              PageSize = pRequest.PageSize,
              TotalRecords = response.TotalRecords,
            };
        }
    }
}
