using ClientManagement.Ai;
using ClientManagement.BusinessLogicLayer;
using ClientManagement.DataAccessLayer;
using ClientManagement.Models;
using ClientManagement.Models.AI;
using ClientManagement.Presentation.Web.Components;
using ClientManagement.Utils;
using ClientManagement.Utils.Constants;
using Core.Presentation.ViewComponents.Utils.DocumentGeneration.Pdf;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Globalization;


namespace ClientManagement.Presentation.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           
            builder.Services.AddSession();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped(typeof(HtmlToPdfConverter));
            builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddCookie(IdentityConstants.ApplicationScheme, c =>
                {
                    c.LoginPath = LoginPathConstants.Login;
                    
                });

            builder.Services.Configure<AppInfo>(builder.Configuration.GetSection("AppInfo"));
            builder.Services.AddUtilServices(builder.Configuration);
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
                config.Timeout = TimeSpan.FromMinutes(3);
            });

           

            var serviceName = "izyBill-Agent-Core";

            builder.Services.AddOpenTelemetry()
                .ConfigureResource(resource => resource.AddService(serviceName))
                .WithTracing(tracing => tracing
                    .AddSource("Microsoft.Extensions.AI")
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddGrpcClientInstrumentation()
                    .AddOtlpExporter(options =>
                     {
                         var telemetryUrl = builder.Configuration["AI:PhoenixUrl"];
                         options.Endpoint = new Uri(telemetryUrl);
                         options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                     })
                  );


            builder.Services.AddControllers(c =>
            {
               //c.Filters.Add(typeof(ApiKeyActionFilter));
            });
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.Configure<AgentOptions>(options =>
            {
               // options = new();
                builder.Configuration.Bind("AI", options);

            });
            builder.Services.AddAiAgents(builder.Configuration);
            builder.Services.AddBusinessServices(builder.Configuration);

            var app = builder.Build();

            app.UseSession();
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

           // app.UseHttpsRedirection();
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
