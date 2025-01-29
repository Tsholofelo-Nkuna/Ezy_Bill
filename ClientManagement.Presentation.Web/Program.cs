using ClientManagement.Presentation.Web.Components;
using Microsoft.EntityFrameworkCore;
using ClientManagement.BusinessLogicLayer;
using ClientManagement.DataAccessLayer;
using Core.Utils.Logging;
using System.Globalization;
using Core.Presentation.ViewComponents.Utils.DocumentGeneration.Pdf;
using ClientManagement.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;

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
            builder.Services
                .AddDbContext<WebDbContext>(c => c.UseSqlServer(builder.Configuration.GetConnectionString("Default")))
                .AddIdentity<IdentityUser, IdentityRole>(c =>
                {
                    
                    c.Password.RequireNonAlphanumeric = false;
                    c.Password.RequireUppercase = false;
                    c.Password.RequireNonAlphanumeric = false;
                    c.Password.RequireLowercase = false;
                    c.Password.RequireDigit = false;
                    c.Password.RequiredLength = 4;
                })
                .AddEntityFrameworkStores<WebDbContext>()
                .AddDefaultTokenProviders();
                
            builder.Services.AddHttpClient("AppApi",config =>
            {
                config.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]); 
            });

            builder
                .Services.AddAuthentication()
                .AddCookie(c =>
                {
                    
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
            //app.UseAuthentication();
            //app.UseAuthorization();
            app.UseStaticFiles();
            app.UseAntiforgery();
            app.MapControllers();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
