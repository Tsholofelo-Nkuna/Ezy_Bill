using ClientManagement.Presentation.Web.Components;
using Microsoft.EntityFrameworkCore;
using ClientManagement.BusinessLogicLayer;
using ClientManagement.DataAccessLayer;
using Core.Utils.Logging;
using System.Globalization;
using Core.Presentation.ViewComponents.Utils.DocumentGeneration.Pdf;
using ClientManagement.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Core.Utils.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Core.Utils;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Core.Utils.Constants;
using Microsoft.AspNetCore.Authentication;
using Core.Utils.ActionFilters;


namespace ClientManagement.Presentation.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.AddProvider(new FileLoggerProvider());
          
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped(typeof(HtmlToPdfConverter));
            builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddCookie(IdentityConstants.ApplicationScheme, c =>
                {
                    c.LoginPath = LoginPathConstants.Login;
                    
                })
               .AddBearerToken(BearerTokenDefaults.AuthenticationScheme);


           
            builder.Services
                .AddDbContext<WebDbContext>(c => c.UseSqlServer(builder.Configuration.GetConnectionString("Default")))
                .AddIdentityCore<IdentityUser>(c =>
                {

                     c.Password.RequireNonAlphanumeric = false;
                     c.Password.RequireUppercase = false;
                     c.Password.RequireNonAlphanumeric = false;
                     c.Password.RequireLowercase = false;
                     c.Password.RequireDigit = false;
                     c.Password.RequiredLength = 4;
                })
                .AddEntityFrameworkStores<WebDbContext>()
                .AddApiEndpoints();
            builder.Services.AddRazorPages();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddHttpClient("AppApi",config =>
            {
                config.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]); 
            });
          

           

           
            builder.Services.AddControllers(c =>
            {
               //c.Filters.Add(typeof(ApiKeyActionFilter));
            });
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddBusinessServices();
         
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
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI();
              
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseAntiforgery();
            app.MapIdentityApi<IdentityUser>();
            app.MapControllers();
            app.MapRazorPages();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();
          
            app.Run();
        }
    }
}
