using ClientManagement.BusinessLogicLayer;
using ClientManagement.DataAccessLayer;
using Core.Utils;
using Core.Utils.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ClientManagement.Presentation.Desktop
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            //builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
            //   .AddCookie(IdentityConstants.ApplicationScheme, c =>
            //   {
            //       c.LoginPath = LoginPathConstants.Login;

            //   });

            //builder.Services.Configure<Core.Presentation.Models.AppInfo>(builder.Configuration.GetSection("AppInfo"));
            //builder.Services.AddUtilServices(builder.Configuration);
            //builder.Services
            //    .AddDbContext<WebDbContext>(c => c.UseSqlServer(builder.Configuration.GetConnectionString("Default")))
            //    .AddIdentityCore<IdentityUser>(c =>
            //    {

            //        c.Password.RequireNonAlphanumeric = false;
            //        c.Password.RequireUppercase = false;
            //        c.Password.RequireNonAlphanumeric = false;
            //        c.Password.RequireLowercase = false;
            //        c.Password.RequireDigit = false;
            //        c.Password.RequiredLength = 4;
            //    })
            //    .AddEntityFrameworkStores<WebDbContext>()
            //    .AddApiEndpoints();
            builder.Services.AddRazorPages();
            builder.Services.AddHttpContextAccessor();

            //builder.Services.AddHttpClient("AppApi", config =>
            //{
            //    config.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]);
            //});





            builder.Services.AddControllers(c =>
            {
                //c.Filters.Add(typeof(ApiKeyActionFilter));
            });
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddBusinessServices();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
