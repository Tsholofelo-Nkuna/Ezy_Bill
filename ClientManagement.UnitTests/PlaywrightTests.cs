using ClientManagement.Presentation.Web.Components.Pages.Invoices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

using Microsoft.Playwright;


namespace ClientManagement.UnitTests
{
    [TestFixture]
    public class PlaywrightTests 
    {

        [Test]
        public void PlayrightPDF_Works()
        {
           
            var parameterValues = ParameterView.FromDictionary(new Dictionary<string, object?>
            {
                { "", "" }
            });
            var c =  new HtmlRenderer(null, null).RenderComponentAsync<Details>(parameterValues).Result;
            var html = c.ToHtmlString();

            var browser = Playwright.CreateAsync().Result.Chromium.LaunchAsync().Result;
            var page = browser.NewPageAsync().Result;
            page.SetContentAsync(html).Wait();
            var bytes = page.PdfAsync().Result;
            
            //page.APIRequest.
            using (var fStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.pdf"), FileMode.OpenOrCreate))
            {
                fStream.Write(bytes, 0, bytes.Length);
            }
            browser.CloseAsync().Wait();

        }


    }
}
