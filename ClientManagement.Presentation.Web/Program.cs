using ClientManagement.Presentation.Web.Components;
using Microsoft.EntityFrameworkCore;
using ClientManagement.BusinessLogicLayer;
using ClientManagement.DataAccessLayer;
using Core.Utils.Logging;
using System.Globalization;
using Core.Presentation.ViewComponents.Utils.DocumentGeneration.Pdf;

namespace ClientManagement.Presentation.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.AddProvider(new FileLoggerProvider());
           // var serviceProvider = builder.Services.BuildServiceProvider();
           // var loggingFactory = serviceProvider.GetService<ILoggerFactory>();

            // Add services to the container.
            builder.Services.AddScoped(typeof(HtmlToPdfConverter));
            builder.Services.AddDbContext<WebDbContext>(c => c.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
            builder.Services.AddHttpClient("AppApi",config =>
            {
                config.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]); 
            });
            builder.Services.AddControllers();
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddBusinessServices();
            builder.Services.AddStateManagers();
            var app = builder.Build();
            app.UseRequestLocalization(options =>
            {
                options.SupportedCultures = new[] { new CultureInfo("en-ZA") };
                options.SupportedUICultures = new[] { new CultureInfo("en-ZA") };
                options.SetDefaultCulture("en-ZA");
                
            });
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();
            app.MapControllers();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
