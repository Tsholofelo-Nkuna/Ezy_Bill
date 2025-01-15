using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    public  class ControllerRequestHandler<TCategory>
    {
		private readonly ILogger<TCategory> _logger;
		public ControllerRequestHandler(ILogger<TCategory> logger) {
		  this._logger = logger;
		}	
        public TOut HandleRequest<TOut>(Func<TOut> requestHandler, string actionName, TOut errorResponse , params object[] actionParameters)
        {
			try
			{
				var response =  requestHandler();
                if (response is Task and { Exception: not null} taskResult)
                {
					throw taskResult.Exception;
                }
			
                return response;
			}
			catch (Exception ex)
			{
				this._logger.LogError($"input(s) passed to {actionName} {actionParameters.ToJsonString()}");
				return errorResponse;
			}
        }
    }
}
