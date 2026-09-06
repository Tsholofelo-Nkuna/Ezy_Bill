using ClientManagement.Models.Base;
using ClientManagement.Models.DataTransferObjects.Base;
using ClientManagement.Models.Interfaces.Base;
using Core.Presentation.ViewComponents.Components.Base;
using Core.Presentation.ViewComponents.Interfaces.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SelectPdf;


namespace Core.Presentation.ViewComponents.Utils.DocumentGeneration.Pdf
{
    public class HtmlToPdfConverter
    {
        private readonly PdfGeneratorOptions _pdfGeneratorOptions;
        private readonly HtmlRenderer _htmlRenderer;
        public HtmlToPdfConverter(IServiceProvider sP) {
           _pdfGeneratorOptions =  new PdfGeneratorOptions(sP.GetService<ILoggerFactory>()!, sP);
            this._htmlRenderer = new HtmlRenderer(_pdfGeneratorOptions.ServiceProvider, _pdfGeneratorOptions.LoggerFactory);
        }

       public async Task<byte[]> CreatePdfAsync(string filePath, IComponent comp, ParameterView compParams)
       {
            
            var html =  await this._htmlRenderer
                .Dispatcher.InvokeAsync( async () =>
                {
                   var component = await this._htmlRenderer.RenderComponentAsync(comp.GetType(), compParams);
                    return component.ToHtmlString();
                });
           
            HtmlToPdf htmlToPdf = new HtmlToPdf();
            var doc = htmlToPdf.ConvertHtmlString(html);
            return doc.Save(); 
        }
    }

    public class PdfGeneratorOptions
    {
        public ILoggerFactory LoggerFactory { get; set; }
        public IServiceProvider ServiceProvider { get; set; }

        public PdfGeneratorOptions(ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            LoggerFactory = loggerFactory;
            ServiceProvider = serviceProvider;
        }
    }

}
