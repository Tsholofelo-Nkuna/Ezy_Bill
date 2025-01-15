using Core.Presentation.ViewComponents;
using Core.Presentation.ViewComponents.Interfaces.Base;
using Microsoft.AspNetCore.Mvc;
using Question3.BusinessLogicLayer;
using Question3.PresentationLayer.Controllers;
using Question3.PresentationLayer.Views.Home;

namespace Question3.PresentationLayer
{
    public static class ControllerExtensions
    {
        public static TServiceResponse 
            HandleServiceRequestErrors<TServiceResponse, TController> 
            (this ControllerBase controller, ILogger<TController> logger, Func<TServiceResponse> requestHandler, params object[] controllerParameters)
            where TController : ControllerBase
        {
			try
			{
                logger.LogInformation("Inputs to controller: {0}", controllerParameters.ToJsonString());
                var result = requestHandler();
                if (result is Task {Exception: not null } or Exception)
                {
                    logger.LogError("Error occured with following input(s) to controller: {0}", controllerParameters.ToJsonString());
                }
                return result;
            }
			catch (Exception)
			{
                throw;
            }
			
        }

        public static void InvokePageInit<TModel>(this ControllerBase controller, IActionResult? pageBase , TModel model)
        {
            
        }
    }
}
