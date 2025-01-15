using Core.Utils;
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
    }
}
